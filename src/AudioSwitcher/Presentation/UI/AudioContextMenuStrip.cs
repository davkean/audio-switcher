// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AudioSwitcher.Presentation.UI.Interop;

namespace AudioSwitcher.Presentation.UI
{
    // Represents a context menu strip that adds additional behavior for audio switcher
    internal class AudioContextMenuStrip : ContextMenuStrip
    {
        private bool _cancelNextAttemptedClose;

        public AudioContextMenuStrip()
        {
            Renderer = new ToolStripNativeRenderer(ToolbarTheme.Toolbar);
            ShowCheckMargin = false;
            ShowImageMargin = true;
        }

        public bool WorkingAreaConstrained
        {
            get;
            set;
        }

        public new void Show(Control control, Point controlLocation)
        {
            // Prevents the context menu from causing the app to show in the taskbar
            DllImports.SetForegroundWindow(new HandleRef(this, Handle));

            Capture = true;
            base.Show(control, controlLocation);
        }

        public void ShowInSystemTray(Point screenLocation)
        {
            // Prevents the context menu from causing the app to show in the taskbar
            DllImports.SetForegroundWindow(new HandleRef(this, Handle));

            if (WorkingAreaConstrained)
            {
                base.Show(screenLocation);
            }
            else
            {
                // HACK: The ContextMenuStrip does some trickery that only the NotifyIcon can call
                // that allows it to be shown outside of the working area of the desktop and over
                // the top of the taskbar. To mimic that same thing, we need to call the same method 
                // that NotifyIcon uses.
                MethodInfo info = typeof(AudioContextMenuStrip).GetMethod("ShowInTaskbar", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new Type[] { typeof(int), typeof(int) }, null);
                Debug.Assert(info != null);
                info.Invoke(this, new object[] { screenLocation.X, screenLocation.Y });
            }
        }

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            AudioToolStripMenuItem item = e.ClickedItem as AudioToolStripMenuItem;
            if (item != null && !item.AutoCloseOnClick)
            {
                _cancelNextAttemptedClose = true;
            }

            base.OnItemClicked(e);
        }

        protected override void OnClosing(ToolStripDropDownClosingEventArgs e)
        {
            if (_cancelNextAttemptedClose && e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
            {
                e.Cancel = true;
                _cancelNextAttemptedClose = false;
                return;
            }

            base.OnClosing(e);
        }

        protected override ToolStripItem CreateDefaultItem(string text, Image image, EventHandler onClick)
        {
            if (text == "-")
            {
                return new ToolStripSeparator();
            }

            var item = new AudioToolStripMenuItem
            {
                Text = text,
                Image = image
            };
            item.Click += onClick;

            return item;
        }
    }
}
