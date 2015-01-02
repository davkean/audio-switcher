// -----------------------------------------------------------------------
// Copyright (c) David Kean and Abdallah Gomah.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace AudioSwitcher.Presentation.Drawing.Utilities
{
    /// <summary>
    /// Holds a set of utilities.
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Reads a structure of type T from the input stream.
        /// </summary>
        /// <typeparam name="T">The structure type to be read.</typeparam>
        /// <param name="inputStream">The input stream to read from.</param>
        /// <returns>A structure of type T that was read from the stream.</returns>
        public static T ReadStructure<T>(Stream inputStream) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            inputStream.Read(buffer, 0, size);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, 0, ptr, size);
            object ret = Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return (T)ret;
        }
        /// <summary>
        /// Writes as structure of type T to the output stream.
        /// </summary>
        /// <typeparam name="T">The structure type to be written.</typeparam>
        /// <param name="outputStream">The output stream to write to.</param>
        /// <param name="structure">The structure to be written.</param>
        public static void WriteStructure<T>(Stream outputStream, T structure) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structure, ptr, true);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);
            outputStream.Write(buffer, 0, size);
        }
    }
}
