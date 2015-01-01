// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;

namespace AudioSwitcher.Presentation.Drawing
{
    internal static class DrawingServices
    {
        public static Image Overlay(Image image, Image overlayImage)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                // Overlay starting in the bottom right-hand corner
                g.DrawImage(overlayImage, image.Width - overlayImage.Width, image.Height - overlayImage.Height, overlayImage.Width, overlayImage.Height);
                g.Save();

                return image;
            }
        }
    }
}
