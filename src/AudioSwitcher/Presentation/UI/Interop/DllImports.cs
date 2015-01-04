// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using AudioSwitcher.Audio.Interop;
using AudioSwitcher.Interop;

namespace AudioSwitcher.Presentation.UI.Interop
{
    internal class DllImports
    {
        [DllImport(ExternalDll.User32, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(HandleRef hWnd);

        [DllImport(ExternalDll.Uxtheme)]
        public extern static int GetThemeMargins(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, int iPropId, IntPtr rect, out MARGINS pMargins);
    }
}
