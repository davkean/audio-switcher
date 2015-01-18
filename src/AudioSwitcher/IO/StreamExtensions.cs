// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace AudioSwitcher.IO
{
    internal static class StreamExtensions
    {
        public static T Read<T>(this Stream stream) where T : struct
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            int bytesRead = stream.Read(buffer, 0, size);

            Debug.Assert(bytesRead == size);

            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(buffer, 0, ptr, size);

            T result = (T)Marshal.PtrToStructure(ptr, typeof(T));

            Marshal.FreeHGlobal(ptr);

            return result;
        }

        public static void Write<T>(this Stream stream, T value) where T : struct
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(value, ptr, true);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);
            stream.Write(buffer, 0, size);
        }
    }
}
