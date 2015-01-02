// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace AudioSwitcher.Presentation.Drawing.Interop
{
    internal class SafeModuleHandle : SafeHandle
    {
        private SafeModuleHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }

        protected override bool ReleaseHandle()
        {
            return DllImports.FreeLibrary(handle);
        }
    }
}
