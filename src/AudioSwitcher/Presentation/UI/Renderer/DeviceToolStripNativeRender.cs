// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AudioSwitcher.Presentation.UI.Renderer
{
    internal class DeviceToolStripNativeRender : ToolStripNativeRenderer
    {
        public DeviceToolStripNativeRender(ToolbarTheme theme)
            : base(theme)
        {
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            string[] text = e.Text.Split(new string[] { Environment.NewLine }, 3, StringSplitOptions.None);

            if (text.Length == 1)
            {   
                // Rendering something other than the device context menu,
                // let base handle it.
                base.OnRenderItemText(e);
            }
            else
            {
                // First render the first line in normal menu text color
                base.OnRenderItemText(new ToolStripItemTextRenderEventArgs(e.Graphics, e.Item, String.Concat(text[0], Environment.NewLine, Environment.NewLine), e.TextRectangle, e.TextColor, e.TextFont, e.TextFormat));

                // Then render, the bottom two lines in gray text
                TextRenderer.DrawText(e.Graphics, String.Concat(Environment.NewLine, text[1], Environment.NewLine, text[2]), e.TextFont, e.TextRectangle, SystemColors.GrayText, e.TextFormat);
            }
        }
    }
}
    
