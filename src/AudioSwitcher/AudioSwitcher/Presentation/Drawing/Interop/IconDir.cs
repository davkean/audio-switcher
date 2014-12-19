using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BYTE = System.Byte;
using WORD = System.Int16;
using DWORD = System.Int32;

namespace AudioSwitcher.Presentation.Drawing
{
    [StructLayout(LayoutKind.Sequential, Size=6)]
    internal struct IconDir
    {
        public WORD Reserved;   // Reserved (must be 0)
        public WORD Type;       // Resource Type (1 for icons)
        public WORD Count;      // How many images?

        public GroupIconDir ToGroupIconDir()
        {
            GroupIconDir grpDir = new GroupIconDir();
            grpDir.Reserved = this.Reserved;
            grpDir.Type = this.Type;
            grpDir.Count = this.Count;
            return grpDir;
        }
    }
}
