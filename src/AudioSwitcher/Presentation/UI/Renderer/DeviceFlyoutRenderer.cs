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
    // A tool strip renderer that handles rendering the device flyout
    internal class DeviceFlyoutRenderer : ToolStripNativeRenderer
    {
        private readonly static string[] NewLine = new[] { Environment.NewLine };
        private readonly VisualStyleElement FlyoutWindow = VisualStyleElement.CreateElement("Flyout", 6, 0);

        public DeviceFlyoutRenderer()
            : base(ToolbarTheme.Toolbar)
        {
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {   // Don't render the image margin
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {   // Don't render border            
        }

		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{	
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
            if (string.IsNullOrEmpty(e.Text)) // Separator
                return;

            string[] text = e.Text.Split(NewLine, 3, StringSplitOptions.None);
            if (text.Length != 3)
            {
                base.OnRenderItemText(e);
                return;
            }

			Debug.Assert(text.Length == 3);

			// First render the first line in normal menu text color
			base.OnRenderItemText(new ToolStripItemTextRenderEventArgs(e.Graphics, e.Item, string.Concat(text[0], Environment.NewLine, Environment.NewLine), e.TextRectangle, e.TextColor, e.TextFont, e.TextFormat));

			// Then render, the bottom two lines in gray text
			TextRenderer.DrawText(e.Graphics, string.Concat(Environment.NewLine, text[1], Environment.NewLine, text[2]), e.TextFont, e.TextRectangle, e.Item.Selected ? SystemColors.HighlightText : SystemColors.GrayText, e.TextFormat);
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

