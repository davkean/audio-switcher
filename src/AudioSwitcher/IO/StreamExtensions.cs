// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace AudioSwitcher.IO
{
    internal static class StreamExtensions
    {
        static internal T Read<T>(this Stream stream) where T : struct
        {
            var size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            Int32 bytesRead = stream.Read(buffer, 0, size);

            if (bytesRead != size)
                throw new InvalidDataException();

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(buffer, 0, ptr, size);

            var result = (T)Marshal.PtrToStructure(ptr, typeof(T));

            Marshal.FreeHGlobal(ptr);

            return result;
        }

        static public void Write<T>(this Stream stream, T value) where T : struct {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr((object)value, ptr, true);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);
            stream.Write(buffer, 0, size);
        }
    }
}
