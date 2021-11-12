// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;
using BYTE = System.Byte;
using WORD = System.Int16;
using DWORD = System.Int32;

namespace AudioSwitcher.Presentation.Drawing
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    internal struct IconDirEntry
    {
        public BYTE Width;          // Width, in pixels, of the image
        public BYTE Height;         // Height, in pixels, of the image
        public BYTE ColorCount;     // Number of colors in image (0 if >=8bpp)
        public BYTE Reserved;       // Reserved ( must be 0)
        public WORD Planes;         // Color Planes
        public WORD BitCount;       // Bits per pixel
        public DWORD BytesInRes;     // How many bytes in this resource?
        public DWORD ImageOffset;    // Where in the file is this image?

        public GroupIconDirEntry ToGroupIconDirEntry(int id)
        {
            var grpEntry = new GroupIconDirEntry();
            grpEntry.Width = Width;
            grpEntry.Height = Height;
            grpEntry.ColorCount = ColorCount;
            grpEntry.Reserved = Reserved;
            grpEntry.Planes = Planes;
            grpEntry.BitCount = BitCount;
            grpEntry.BytesInRes = BytesInRes;
            grpEntry.ID = (short)id;
            return grpEntry;
        }
    }
}
