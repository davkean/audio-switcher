// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

using WORD = System.Int16;

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
            var dir = new IconDir();
            dir.Reserved = Reserved;
            dir.Type = Type;
            dir.Count = Count;
            return dir;
        }
    }
}
