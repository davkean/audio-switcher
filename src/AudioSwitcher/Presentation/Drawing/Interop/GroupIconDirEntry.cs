using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using BYTE = System.Byte;
using WORD = System.Int16;
using DWORD = System.Int32;

namespace AudioSwitcher.Presentation.Drawing
{
    [StructLayout(LayoutKind.Sequential, Size=14)]
    internal struct GroupIconDirEntry
    {
        public BYTE  Width;          // Width, in pixels, of the image
        public BYTE  Height;         // Height, in pixels, of the image
        public BYTE  ColorCount;     // Number of colors in image (0 if >=8bpp)
        public BYTE  Reserved;       // Reserved ( must be 0)
        public WORD  Planes;         // Color Planes
        public WORD  BitCount;       // Bits per pixel
        public DWORD BytesInRes;     // How many bytes in this resource?
        public WORD  ID;             // the ID

        public IconDirEntry ToIconDirEntry(int imageOffiset)
        {
            IconDirEntry entry = new IconDirEntry();
            entry.Width = this.Width;
            entry.Height = this.Height;
            entry.ColorCount = this.ColorCount;
            entry.Reserved = this.Reserved;
            entry.Planes = this.Planes;
            entry.BitCount = this.BitCount;
            entry.BytesInRes = this.BytesInRes;
            entry.ImageOffset = imageOffiset;
            return entry;
        }
    }
}
