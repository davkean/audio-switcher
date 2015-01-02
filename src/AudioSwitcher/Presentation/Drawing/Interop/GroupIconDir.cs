using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BYTE = System.Byte;
using WORD = System.Int16;
using DWORD = System.Int32;

namespace AudioSwitcher.Presentation.Drawing
{
    /// <summary>
    /// Presents a Group Icon Directory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size=6)]
    internal struct GroupIconDir
    {
        public WORD Reserved;   // Reserved (must be 0)
        public WORD Type;       // Resource Type (1 for icons)
        public WORD Count;      // How many images?

        public IconDir ToIconDir()
        {
            IconDir dir = new IconDir();
            dir.Reserved = this.Reserved;
            dir.Type = this.Type;
            dir.Count = this.Count;
            return dir;
        }
    }
}
