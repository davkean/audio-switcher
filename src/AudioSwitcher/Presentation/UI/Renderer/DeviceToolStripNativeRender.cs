// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace AudioSwitcher.Presentation.UI.Renderer
{
    internal class DeviceToolStripNativeRender : ToolStripNativeRenderer
    {
        private readonly static string[] NewLine = new[] { Environment.NewLine };
        private readonly VisualStyleElement FlyoutWindow = VisualStyleElement.CreateElement("Flyout", 6, 0);

        public DeviceToolStripNativeRender()
            : base(ToolbarTheme.Toolbar)
        {
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {   // Don't render the image margin
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {   // Don't render border            
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (EnsureRenderer())
            {
                Renderer.SetParameters(FlyoutWindow);
                Renderer.DrawBackground(e.Graphics, e.AffectedBounds);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Text.Length == 0) // Separator
                return;

            string[] text = e.Text.Split(NewLine, 3, StringSplitOptions.None);

            Debug.Assert(text.Length == 3);

            // First render the first line in normal menu text color
            base.OnRenderItemText(new ToolStripItemTextRenderEventArgs(e.Graphics, e.Item, String.Concat(text[0], Environment.NewLine, Environment.NewLine), e.TextRectangle, e.TextColor, e.TextFont, e.TextFormat));

            // Then render, the bottom two lines in gray text
            TextRenderer.DrawText(e.Graphics, String.Concat(Environment.NewLine, text[1], Environment.NewLine, text[2]), e.TextFont, e.TextRectangle, SystemColors.GrayText, e.TextFormat);
        }

        protected override Rectangle GetBackgroundRectangle(ToolStripItem item)
        {
            Rectangle rect = item.Bounds;
            rect.X = item.ContentRectangle.X;
            rect.Width = item.ContentRectangle.Width + 1;
            rect.Y = 0;

            return rect;
        }
    }
}

