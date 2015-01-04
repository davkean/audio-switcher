using System;
using AudioSwitcher.Win32.InteropServices;

namespace AudioSwitcher.Interop
{
    internal static class HResult
    {
        public static readonly int OK = 0;
        public static readonly int NotFound = unchecked((int)0x80070490);
    }
}
