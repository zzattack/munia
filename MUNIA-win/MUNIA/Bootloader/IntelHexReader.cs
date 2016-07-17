using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MUNIA.Bootloader {
	public class IntelHexFile {
		private readonly StreamReader _sr;
		// key: block index
		public const int BlockSize = 64;
		private readonly Dictionary<uint, byte[]> _memoryMap = new Dictionary<uint, byte[]>();
		public readonly List<Chunk> Chunks = new List<Chunk>();

		public IntelHexFile(string path) {
			_sr = new StreamReader(path);
			Parse();
		}

		public class Chunk {
			public readonly List<byte> Data = new List<byte>();
			public uint Address { get; set; }
			public uint AddressEnd => (uint)(Address + Data.Count);

			public override string ToString() {
				return $"Chunk 0x{Address:X4}-{AddressEnd:X4}";
			}
		}

		public enum RecordType : byte {
			DataRecord = 0x00,
			EndOfFile = 0x01,
			ExtendedSegmentAddress = 0x02,
			StartSegmentAddress = 0x03,
			ExtendedLinearAddress = 0x04,
			StartLinearAddress = 0x05
		}

		public class Line {
			public byte ByteCount;
			public ushort Address;
			public RecordType RecordType;
			public byte[] Data;
			public byte Checksum;
			public bool Valid;

			private bool IsValid() {
				int sum = 0x100;
				sum -= ByteCount;
				sum -= (Address & 0xFF);
				sum -= ((Address >> 8) & 0xFF);
				sum -= (byte)RecordType;
				foreach (byte b in Data)
					sum -= b;
				return ((byte)(sum & 0xFF)) == Checksum;
			}

			public Line(string line) {
				Valid = false;
				if (line.Length < 11) return;
				if (line[0] != ':') return;
				try {
					ByteCount = Convert.ToByte(line.Substring(1, 2), 16);
					Address = Convert.ToUInt16(line.Substring(3, 4), 16);
					RecordType = (RecordType)Convert.ToByte(line.Substring(7, 2), 16);
					Checksum = Convert.ToByte(line.Substring(line.Length - 2), 16);
					Data = new byte[ByteCount];
					for (int i = 9; i < 9 + ByteCount * 2; i += 2) {
						byte b = Convert.ToByte(line.Substring(i, 2), 16);
						Data[(i - 9) / 2] = b;
					}
					Valid = IsValid();
				}
				catch (FormatException) { }
			}
		}

		public int Size { get; private set; }

		private void Parse() {
			if (_sr.BaseStream == null || _sr.EndOfStream) return;

			uint currentAddress = 0x00000000;

			while (!_sr.EndOfStream && _sr.BaseStream != null) {
				string rl = _sr.ReadLine();
				if (rl == null) break;
				var l = new Line(rl);

				if (!l.Valid) continue; // probably garbage

				if (l.RecordType == RecordType.EndOfFile) {
					_sr.Close();
					_sr.Dispose();
					BuildChunks();
					break;
				}

				if (l.RecordType == RecordType.ExtendedLinearAddress) {
					// The two data bytes (two hex digit pairs in big endian order) represent the upper
					// 16 bits of the 32 bit address for all subsequent 00 type records until the next 04 type record 
					currentAddress = (uint)(BitConverter.ToUInt16(l.Data, 0) << 16);
				}

				else if (l.RecordType == RecordType.DataRecord) {
					currentAddress &= 0xFFFF0000;
					currentAddress |= l.Address;
					WriteMemory(currentAddress, l.Data);
				}
			}

			Size = 0;
			foreach (var block in _memoryMap)
				Size += block.Value.Length;
		}

		private void WriteMemory(uint address, IList<byte> data) {
			// find start block
			var block = GetBlock(address);
			int blockStart = (int)(address & (BlockSize-1));

			for (int i = 0; i < data.Count; i++) {
				if (i + blockStart == block.Length) {
					// block full, get next
					block = GetBlock((uint)(address + i));
					blockStart = -i;
				}

				block[i + blockStart] = data[i];
			}
		}

		private byte[] GetBlock(uint address) {
			uint addressStart = (uint)(address & ~(BlockSize-1));
			byte[] block;
			if (!_memoryMap.TryGetValue(addressStart / BlockSize, out block)) {
				block = _memoryMap[addressStart / BlockSize] = new byte[BlockSize];
				for (int i = 0; i < block.Length; i++)
					block[i] = (byte)(i % 2 == 1 ? 0x00 : 0x00);
			}
			return block;
		}

		/// <summary>
		///     builds chunks from the memory map
		/// </summary>
		private void BuildChunks() {
			Chunk currentChunk = null;
			uint chunkPointer = 0xffffffff;

			foreach (var kvp in _memoryMap.OrderBy(k => k.Key)) {
				uint address = kvp.Key * BlockSize;


				if (address != chunkPointer) {
					// pad now-finished chunk on 64b
					if (currentChunk != null) {
						int len = (currentChunk.Data.Count + BlockSize - 1) & ~(BlockSize-1);
						if (len != currentChunk.Data.Count)
							currentChunk.Data.AddRange(new byte[len - currentChunk.Data.Count]);
					}

					currentChunk = new Chunk();
					// new chunk starts here. insert nops below closest 64b boundary below.
					currentChunk.Address = address;
					Chunks.Add(currentChunk);
					chunkPointer = address;
				}


				if (currentChunk != null) {
					currentChunk.Data.AddRange(kvp.Value);
					chunkPointer += (uint)kvp.Value.Length;
				}

			}

		}

		// prints memory to debug window
		private void PrintMemory() {
			var memory = _memoryMap.OrderBy(k => k.Key);
			foreach (var kvp in memory) {
				Debug.WriteLine("0x{0:X4} :: {1}", kvp.Key * BlockSize,
					string.Join(" ", kvp.Value.Select(x => x.ToString("X")).ToArray()));
			}
		}

		public byte[] MapMemory(uint memStart, uint count, uint bytesPerAddress = 1) {
			var data = new byte[count * bytesPerAddress];
			for (int i = 0; i < data.Length; i++) data[i] = 0xff;

			ulong startPicAddress = memStart * bytesPerAddress;
			ulong endPicAddress = startPicAddress + (count * bytesPerAddress);

			// Assumes blocks are ordered by address
			foreach (var block in _memoryMap) {
				ulong addr = block.Key * BlockSize;
				for (uint i = 0; i < block.Value.Length; i++) {
					if (addr >= startPicAddress && addr < endPicAddress) {
						data[addr - startPicAddress] = block.Value[i];
					}
					addr++;
				}
			}

			return data;
		}
	}
}