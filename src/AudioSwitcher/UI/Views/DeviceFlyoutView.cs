// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;
using AudioSwitcher.Presentation.UI.Renderer;

namespace AudioSwitcher.Presentation.UI.Views
{
    // Represents the flyout window that lists the audio devices, it's a context menu with thick
    // non-resizable borders, to make it look like the other notification windows, such as Volume 
    // and Action Center.
    internal class DeviceFlyoutView : AudioContextMenuStrip
    {
        private const int WM_NCHITTEST = 0x0084;
        private const int WS_THICKFRAME = 0x00040000;
        private const int HTCLIENT = 0x1;

        public DeviceFlyoutView()
        {
            DropShadowEnabled = false;
            Renderer = new DeviceToolStripNativeRender();
            AutoCloseOnContextMenuShow = false;
            WorkingAreaConstrained = true;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            proposedSize = base.GetPreferredSize(proposedSize);

            // Account for windows borders
            return new Size(proposedSize.Width + (Size.Width - ClientSize.Width), proposedSize.Height + (Size.Height - ClientSize.Height));
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams parameters = base.CreateParams;

                // Add resize window frame to mimic other notification
                // windows such as Volume and Action center
                parameters.Style |= (int)WS_THICKFRAME;

                return parameters;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST)
            {
                // Treat hit tests over non-client areas as 
                // handled to prevent resize of the borders
                if (!IsOverClientArea(m.HWnd, m.WParam, m.LParam))
                    return;
            }

            base.WndProc(ref m);
        }

        private bool IsOverClientArea(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
        {
            Message m = new Message();
            m.HWnd = hWnd;
            m.Msg = WM_NCHITTEST;
            m.WParam = wParam;
            m.LParam = lParam;

            base.WndProc(ref m);

            return m.Result == (IntPtr)HTCLIENT;
        }
    }
}
