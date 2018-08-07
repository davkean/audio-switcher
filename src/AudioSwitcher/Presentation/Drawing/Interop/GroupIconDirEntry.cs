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
            var entry = new IconDirEntry
            {
                Width = Width,
                Height = Height,
                ColorCount = ColorCount,
                Reserved = Reserved,
                Planes = Planes,
                BitCount = BitCount,
                BytesInRes = BytesInRes,
                ImageOffset = imageOffiset
            };
            return entry;
        }
    }
}
