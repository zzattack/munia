using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using HidSharp;

namespace MUNIA.Bootloader {

	public class HidBootloader {
		public HidDevice HidDevice;

		MemoryRegionStruct[] _memoryRegions;
		byte _progressStatus;
		bool _unlockStatus;
		byte _bytesPerAddress;
		byte _bytesPerPacket;
		DeviceFamilyType _deviceFamily;

		public MemoryRegionStruct[] MemoryRegions => _memoryRegions;
		public DeviceFamilyType DeviceFamily => _deviceFamily;

		public const int MaxDataRegions = 6;
		const int CommandReportSize = 65;
		const int ProgramPacketDataSize = 58;

		public enum BootloaderCommand : byte { Query = 0x02, UnlockConfig = 0x03, Erase = 0x04, Program = 0x05, ProgramComplete = 0x06, GetData = 0x07, Reset = 0x08, SignFlash = 0x09, GetEncryptedFF = 0xFF }
		public enum QueryResult : byte { Idle = 0xFF, Running = 0x00, Success = 0x01, WriteFailed = 0x02, ReadFailed = 0x03, MallocFailed = 0x04 }
		public enum ProgramResult : byte { Idle = 0xFF, Running = 0x00, Success = 0x01, WriteFailed = 0x02, ReadFailed = 0x03, MallocFailed = 0x04, RunningErase = 0x05, RunningProgram = 0x06 }
		public enum EraseResult : byte { Idle = 0xFF, Running = 0x00, Success = 0x01, WriteFailed = 0x02, ReadFailed = 0x03, MallocFailed = 0x04, PostQueryFailure = 0x05, PostQueryRunning = 0x06, PostQuerySuccess = 0x07 }
		public enum VerifyResults : byte { Idle = 0xFF, Running = 0x00, Success = 0x01, WriteFailed = 0x02, ReadFailed = 0x03, Mismatch = 0x04 }
		public enum ReadResult : byte { Idle = 0xFF, Running = 0x00, Success = 0x01, WriteFailed = 0x02, ReadFailed = 0x03 }
		public enum UnlockConfigResult : byte { Idle = 0xFF, Running = 0x00, Success = 0x01, Failure = 0x02 }
		public enum BootloaderState { Idle = 0xFF, Query = 0x00, Program = 0x01, Erase = 0x02, Verify = 0x03, Read = 0x04, UnlockConfig = 0x05, Reset = 0x06 }
		public enum ResetResults : byte { Idle = 0xFF, Running = 0x00, Success = 0x01, WriteFailed = 0x02 }
		public enum MemoryRegionType : byte { Flash = 0x01, EEPROM = 0x02, ConfigBits = 0x03, UserId = 0x04, End = 0xFF }
		public enum DeviceFamilyType : byte { PIC18 = 1, PIC24 = 2, PIC32 = 3 }

		#region Structs

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct MemoryRegionStruct {
			public MemoryRegionType Type;
			public UInt32 Address;
			public UInt32 Size;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct EnterBootloaderStruct {
			public byte ReportID;
			public BootloaderCommand Command;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct QueryDeviceStruct {
			public byte ReportID;
			public BootloaderCommand Command;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct QueryResultsStruct {
			public byte ReportID;
			public BootloaderCommand Command;
			public byte BytesPerPacket;
			public DeviceFamilyType DeviceFamily;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxDataRegions)]
			public MemoryRegionStruct[] MemoryRegions;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct UnlockConfigStruct {
			public byte ReportID;
			public BootloaderCommand Command;
			public byte Setting;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct SignFlashStruct {
			public byte ReportID;
			public BootloaderCommand Command;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct EraseDeviceStruct {
			public byte WindowsReserved;
			public byte Command;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct ProgramDeviceStruct {
			public byte ReportID;
			public BootloaderCommand Command;
			public UInt32 Address;
			public byte BytesPerPacket;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = ProgramPacketDataSize)]
			public byte[] Data;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct ProgramCompleteStruct {
			public byte WindowsReserved;
			public byte Command;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct GetDataStruct {
			public byte ReportID;
			public byte Command;
			public UInt32 Address;
			public byte BytesPerPacket;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct GetDataResultsStruct {
			public byte ReportID;
			public BootloaderCommand Command;
			public UInt32 Address;
			public byte BytesPerPacket;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 58)]
			public byte[] Data;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct ResetDeviceStruct {
			public byte ReportID;
			public BootloaderCommand Command;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct GetEncryptedFFResultsStruct {
			public byte ReportID;
			public BootloaderCommand Command;
			public byte blockSize;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 63)]
			public byte[] Data;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct PacketDataStruct {
			public byte ReportID;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
			public byte[] Data;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1, Size = CommandReportSize)]
		public struct BootloaderCommandStruct {
			public byte ReportID;
			public BootloaderCommand Command;
		}

		#endregion

		public HidBootloader(HidDevice dev) {
			HidDevice = dev;

			_memoryRegions = new MemoryRegionStruct[MaxDataRegions];

			_unlockStatus = false;

			//Set the progress status bar to 0%
			_progressStatus = 0;

			//Set the number of bytes per address to 0 until we perform
			//	a query and get the real results
			_bytesPerAddress = 0;
		}

		#region Device Commands

		/// <summary>
		/// This function queries the attached device for the programmable memory regions
		/// and stores the information returned into the memoryRegions array.
		/// </summary>
		public void Query() {
			if (HidDevice.DevicePath == null)
				throw new NullReferenceException("HID device not connected");

			//Create the write file and read file handles the to the USB device
			//  that we want to talk to
			using (var stream = HidDevice.Open()) {
				QueryDeviceStruct myCommand = new QueryDeviceStruct();
				QueryResultsStruct myResponse = new QueryResultsStruct();

				// Prepare the command that we want to send, in this case the QUERY device command
				myCommand.ReportID = 0;
				myCommand.Command = BootloaderCommand.Query;

				// Send the command that we prepared
				stream.WriteStructure(myCommand);

				// Try to read a packet from the device
				myResponse = stream.ReadStructure<QueryResultsStruct>();

				// for each of the possible memory regions
				var memRegions = new List<MemoryRegionStruct>();
				for (byte i = 0; i < MaxDataRegions; i++) {
					//If the type of region is 0xFF that means that we have
					//  reached the end of the regions array.
					if (myResponse.MemoryRegions[i].Type == MemoryRegionType.End)
						break;

					//copy the data from the packet to the local memory regions array
					memRegions.Add(myResponse.MemoryRegions[i]);
				}
				_memoryRegions = memRegions.ToArray();

				//copy the last of the data out of the results packet
				switch (myResponse.DeviceFamily) {
					case DeviceFamilyType.PIC18:
						_bytesPerAddress = 1;
						break;
					case DeviceFamilyType.PIC24:
						_bytesPerAddress = 2;
						break;
					case DeviceFamilyType.PIC32:
						_bytesPerAddress = 1;
						break;
				}
				_deviceFamily = myResponse.DeviceFamily;
				_bytesPerPacket = myResponse.BytesPerPacket;
			}

		}

		#endregion

		#region Basic Commands
		/// <summary>
		/// Send a generic packet with no response
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="packet"></param>
		private void SendCommandPacket<T>(T packet) where T : struct {
			using (var pipe = HidDevice.Open()) {
				SendCommandPacket(pipe, packet);
			}
		}
		private void SendCommandPacket<T>(HidStream writeFile, T packet) where T : struct {
			writeFile.WriteStructure(packet);
		}

		/// <summary>
		/// Resets the target device
		/// </summary>
		public void Reset() {
			using (var pipe = HidDevice.Open()) {
				pipe.WriteStructure(new BootloaderCommandStruct {
					ReportID = 0,
					Command = BootloaderCommand.Reset
				});
			}
		}

		/// <summary>
		/// Erases the target device
		/// </summary>
		public void Erase() {
			using (var pipe = HidDevice.Open()) {
				pipe.WriteStructure(new BootloaderCommandStruct {
					ReportID = 0,
					Command = BootloaderCommand.Erase
				});
			}

			WaitForCommand();
		}

		/// <summary>
		/// Unlocks or Locks the target device's config bits for writing
		/// </summary>
		private void UnlockConfigBits(bool lockBits) {
			using (var WriteDevice = HidDevice.Open()) {
				WriteDevice.WriteStructure(new UnlockConfigStruct {
					ReportID = 0,
					Command = BootloaderCommand.UnlockConfig,

					//0x00 is sub-command to unlock the config bits
					//0x01 is sub-command to lock the config bits
					Setting = (lockBits) ? (byte)0x01 : (byte)0x00
				});
			}
		}

		/// <summary>
		/// Waits for the previous command to complete, by sending a test Query packet.
		/// </summary>
		private void WaitForCommand() {
			using (var WriteFile = HidDevice.Open()) {
				//If we were able to successfully send the erase command to
				//  the device then let's prepare a query command to determine
				//  when the is responding to commands again
				WriteFile.WriteStructure(new QueryDeviceStruct {
					ReportID = 0,
					Command = BootloaderCommand.Query
				});
			}

			using (var pipe = HidDevice.Open()) {
				//Try to read a packet from the device
				pipe.ReadStructure<QueryResultsStruct>();
			}
		}

		#endregion

		#region Programming Commands

		public IntelHexFile Read() {
			return null;
		}

		/// <summary>
		/// Program the target device with the provided hexfile
		/// </summary>
		/// <param name="hexFile">Hexfile containing data to program</param>
		/// <param name="programConfigs">If true, will attempt to program config words (WARNING: programming invalid config words could brick the device!)</param>
		public void Program(IntelHexFile hexFile, bool programConfigs = false) {
			// Program config words first to minimise the risk that the MCU
			// is reset during programming, thus leaving the MCU in a state 
			// that can't be booted.
			if (programConfigs) {
				var configRegions = _memoryRegions.Where(r => r.Type == MemoryRegionType.ConfigBits);

				// Not all devices provide CONFIG memory regions, as it is usually not desirable to program them anyway.
				if (!configRegions.Any())
					throw new ArgumentException("Cannot program config words for this device (No CONFIG memory regions)", nameof(hexFile));

				foreach (var memoryRegion in configRegions) {
					ProgramMemoryRegion(hexFile, memoryRegion);
				}
			}

			// Program everything else (PROGMEM, EEDATA)
			var dataRegions = _memoryRegions.Where(r => r.Type == MemoryRegionType.Flash);

			// This shouldn't happen in a properly configured device, but show in case it does to prevent confusion
			if (!dataRegions.Any())
				throw new ArgumentException("Cannot program memory (No PROGMEM/EEDATA memory regions)", nameof(hexFile));

			_totalFlashSize = dataRegions.Sum(r => r.Size);
			FlashCount = 0;
			foreach (var memoryRegion in dataRegions) {
				ProgramMemoryRegion(hexFile, memoryRegion);
			}
		}

		private void ProgramMemoryRegion(IntelHexFile hexFile, MemoryRegionStruct memoryRegion) {
			using (var pipe = HidDevice.Open()) {
				byte currentByteInAddress = 1;
				bool skippedBlock = false;

				// Obtain the data related to the current memory region
				var regionData = hexFile.MapMemory(memoryRegion.Address, memoryRegion.Size, _bytesPerAddress);
				int j = 0;

				// While the current address is less than the end address
				uint address = memoryRegion.Address;
				uint endAddress = memoryRegion.Address + memoryRegion.Size;
				while (address < endAddress) {
					// Prepare command
					ProgramDeviceStruct myCommand = new ProgramDeviceStruct {
						ReportID = 0,
						Command = BootloaderCommand.Program,
						Address = address
					};
					myCommand.Data = new byte[ProgramPacketDataSize];

					// If a block consists of all 0xFF, then there is no need to write the block
					// as the erase cycle will have set everything to 0xFF
					bool skipBlock = true;

					byte i;
					for (i = 0; i < _bytesPerPacket; i++) {
						byte data = regionData[j++];

						myCommand.Data[i + (myCommand.Data.Length - _bytesPerPacket)] = data;

						if (data != 0xFF) {
							// We can skip a block if all bytes are 0xFF.
							// Bytes are also ignored if it is byte 4 of a 3 word instruction on PIC24 (bytesPerAddress=2, currentByteInAddress=2, even address)

							if ((_bytesPerAddress != 2) || ((address % 2) == 0) || (currentByteInAddress != 2)) {
								// Then we can't skip this block of data
								skipBlock = false;
							}
						}

						if (currentByteInAddress == _bytesPerAddress) {
							// If we haven't written enough bytes per address to be at the next address
							address++;
							currentByteInAddress = 1;
						}
						else {
							// If we haven't written enough bytes to fill this address
							currentByteInAddress++;
						}

						//If we have reached the end of the memory region, then we
						//  need to pad the data at the end of the packet instead
						//  of the front of the packet so we need to shift the data
						//  to the back of the packet.
						if (address >= endAddress) {
							byte n;
							i++;

							int len = myCommand.Data.Length;
							for (n = 0; n < len; n++) {
								if (n < i)
									// Move it from where it is to the back of the packet, thus shifting all of the data down.
									myCommand.Data[len - n - 1] = myCommand.Data[i + (len - _bytesPerPacket) - n - 1];
								else
									myCommand.Data[len - n - 1] = 0x00;
							}

							// Break out of the for loop now that all the data has been padded out.
							break;
						}

					}//end for

					// Use the counter to determine how many bytes were written
					myCommand.BytesPerPacket = i;

					//If the block was all 0xFF then we can just skip actually programming
					//  this device.  Otherwise enter the programming sequence
					if (!skipBlock) {
						//If we skipped one block before this block then we may need
						//  to send a proramming complete command to the device before
						//  sending the data for this command.
						if (skippedBlock) {
							SendCommandPacket(pipe, new BootloaderCommandStruct {
								ReportID = 0,
								Command = BootloaderCommand.ProgramComplete
							});

							//since we have now indicated that the programming is complete
							//  then we now mark that we haven't skipped any blocks
							skippedBlock = false;
						}

						SendCommandPacket(pipe, myCommand);
						FlashCount += ProgramPacketDataSize;
					}
					else {
						// We are skipping the block
						skippedBlock = true;
						FlashCount += ProgramPacketDataSize; 
					}
				}//end while

				// All data for this region has been programmed
				SendCommandPacket(pipe, new BootloaderCommandStruct {
					ReportID = 0,
					Command = BootloaderCommand.ProgramComplete
				});
				ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(0, null));

			}//end using
		}


		public bool Verify(IntelHexFile hex) {
			return true;
		}

		/// <summary>
		/// Signs the program so that upon next boot the bootloader will detect
		/// the firmware is written correctly.
		/// </summary>
		public void SignFlash() {
			SendCommandPacket(new SignFlashStruct {
				ReportID = 0,
				Command = BootloaderCommand.SignFlash
			});
		}
		#endregion


		#region progress stuff
		public event ProgressChangedEventHandler ProgressChanged;
		long _totalFlashSize;
		private long _flashCount;

		private long FlashCount {
			get { return _flashCount; }
			set {
				_flashCount = value;
				if (ProgressChanged != null) {
					int p = _totalFlashSize <= 0 ? 0 : (int)(100.0 * _flashCount / _totalFlashSize);
					var args = new ProgressChangedEventArgs(p, null);
					ProgressChanged.Invoke(this, args);
				}
			}
		}
		#endregion
	}

}
