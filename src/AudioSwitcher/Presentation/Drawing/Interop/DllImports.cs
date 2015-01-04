// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using AudioSwitcher.Audio.Interop;
using AudioSwitcher.Interop;

namespace AudioSwitcher.Presentation.Drawing.Interop
{
    internal class DllImports
    {
        [DllImport(ExternalDll.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern SafeModuleHandle LoadLibraryEx(string lpFileName, IntPtr hFile, LoadLibraryExFlags dwFlags);

        [DllImport(ExternalDll.Kernel32, SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport(ExternalDll.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool EnumResourceNames(SafeHandle hModule, ResourceTypes lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam);

        [DllImport(ExternalDll.Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern IntPtr FindResource(SafeModuleHandle hModule, IntPtr lpName, ResourceTypes lpType);

        [DllImport(ExternalDll.Kernel32, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LoadResource(SafeModuleHandle hModule, IntPtr hResInfo);

        [DllImport(ExternalDll.Kernel32, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport(ExternalDll.Kernel32, SetLastError = true, ExactSpelling = true)]
        public static extern int SizeofResource(SafeModuleHandle hModule, IntPtr hResInfo);

        [DllImport(ExternalDll.User32, SetLastError = true, ExactSpelling = true)]
        public static extern int LookupIconIdFromDirectory(IntPtr presbits, bool fIcon);

        [DllImport(ExternalDll.User32, SetLastError = true, ExactSpelling = true)]
        public static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, bool fIcon, int cxDesired, int cyDesired, LookupIconIdFromDirectoryExFlags Flags);
    }
}
