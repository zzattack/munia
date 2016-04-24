using System.Runtime.InteropServices;
using HidSharp;

namespace MUNIA.Bootloader {
	static class ExtensionMethods {
		public static void WriteStructure<T>(this HidStream s, T packet) where T : struct {
			// Auto size if not specified
			int size = Marshal.SizeOf(typeof(T));

			// Convert struct to byte array
			byte[] bytes = new byte[size];
			GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			try {
				Marshal.StructureToPtr(packet, handle.AddrOfPinnedObject(), false);
			}
			finally {
				handle.Free();
			}

			// Write
			s.Write(bytes, 0, size);
		}
		public static T ReadStructure<T>(this HidStream s) {
			// Auto size if not specified
			int size = Marshal.SizeOf(typeof(T));

			// Allocate memory and read
			byte[] bytes = new byte[size];
			s.Read(bytes, 0, size);

			// Convert to target struct
			// Note that it does not copy the data, it assigns the struct to the same memory as the byte array.
			GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			try {
				T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
				return stuff;
			}
			finally {
				handle.Free();
			}
		}
	}
}