// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using AudioSwitcher.Interop;

namespace AudioSwitcher.Presentation.UI.Interop
{
    internal class DllImports
    {
        [DllImport(ExternalDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);
    }
}
