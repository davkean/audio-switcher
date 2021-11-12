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
            var grpDir = new GroupIconDir();
            grpDir.Reserved = Reserved;
            grpDir.Type = Type;
            grpDir.Count = Count;
            return grpDir;
        }
    }
}
