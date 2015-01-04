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

            using (Graphics g = Graphics.FromImage(copy))
            {
                g.DrawImage(image, 0, 0);
                // Overlay starting in the bottom right-hand corner
                g.DrawImage(overlayImage, size.Width - overlayImage.Width, size.Height - overlayImage.Height, overlayImage.Width, overlayImage.Height);
                g.Save();

                return copy;
            }
        }
    }
}
