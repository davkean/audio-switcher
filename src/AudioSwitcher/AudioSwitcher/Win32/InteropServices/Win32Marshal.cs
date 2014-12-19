// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace AudioSwitcher.Win32.InteropServices
{
    internal static class Win32Marshal
    {
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_BAD_EXE_FORMAT = 193;

        public static Exception GetExceptionForLastWin32Error(string fileName)
        {
            int errorCode = Marshal.GetLastWin32Error();
            
            switch (errorCode)
            {
                case ERROR_FILE_NOT_FOUND:
                    throw new FileNotFoundException(null, fileName);

                case ERROR_BAD_EXE_FORMAT:
                    throw new BadImageFormatException();

                default:
                    throw new Win32Exception(errorCode);
            }
        }
    }
}
