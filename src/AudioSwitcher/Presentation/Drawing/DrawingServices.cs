// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;

namespace AudioSwitcher.Presentation.Drawing
{
    internal static class DrawingServices
    {
        public static Image CreateOverlayedImage(Image image, Image overlayImage, Size size)
        {
            if (image == null || overlayImage == null)
                throw new ArgumentNullException(image == null ? "image" : "overlayImage");

            Image copy = new Bitmap(size.Width, size.Height);

            using (var g = Graphics.FromImage(copy))
            {
                g.DrawImage(image, 0, 0);
                g.DrawImage(overlayImage, 0, 0);
                g.Save();

                return copy;
            }
        }
    }
}
