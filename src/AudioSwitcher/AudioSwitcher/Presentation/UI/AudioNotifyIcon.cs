// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AudioSwitcher.Presentation.UI
{
    // A facade around a notify icon that adds a left-click context menu
    internal class AudioNotifyIcon : IDisposable
    {
        private readonly NotifyIcon _icon = new NotifyIcon();
        private AudioContextMenu _leftClickContextMenuStrip;
        
        public AudioNotifyIcon()
        {
            _icon.Visible = true;
            _icon.MouseUp += OnMouseUp;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (LeftClickContextMenuStrip != null && e.Button == MouseButtons.Left)
                LeftClickContextMenuStrip.ShowInSystemTray(Cursor.Position);
        }

        public string Title
        {
            get { return _icon.Text; }
            set { _icon.Text = value; }
        }

        public Icon Icon
        {
            get { return _icon.Icon; }
            set { _icon.Icon = value; }
        }

        public AudioContextMenu LeftClickContextMenuStrip
        {
            get { return _leftClickContextMenuStrip; }
            set { _leftClickContextMenuStrip = value; }
        }

        public AudioContextMenu RightClickContextMenuStrip
        {
            get { return (AudioContextMenu)_icon.ContextMenuStrip; }
            set { _icon.ContextMenuStrip = value; }
        }

        public void Dispose()
        {
            _icon.MouseUp -= OnMouseUp;
            _icon.Dispose();
        }
    }
}
