// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

using WORD = System.Int16;

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
