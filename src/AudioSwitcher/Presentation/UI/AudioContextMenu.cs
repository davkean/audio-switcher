// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AudioSwitcher.Presentation.UI.Interop;

namespace AudioSwitcher.Presentation.UI
{
    // Represents a context menu strip that adds additional behavior for audio switcher
    internal class AudioContextMenu : ContextMenuStrip
    {
        private bool _isDynamic;
        private bool _autoCloseWhenItemWithDropDownClicked;

        public AudioContextMenu()
        {
            Renderer = new ToolStripNativeRenderer(ToolbarTheme.Toolbar) { RenderArrowOnDisabledItems = false };
            ShowCheckMargin = false;
        }

        public void ShowInSystemTray(Point screenLocation)
        {
            if (Visible)
            {
                this.Close(ToolStripDropDownCloseReason.AppFocusChange);
            }
            else
            {

                // Prevents the context menu from causing the app to show in the taskbar
                DllImports.SetForegroundWindow(new HandleRef(this, Handle));
                base.Show(screenLocation, ToolStripDropDownDirection.AboveLeft);
            }
        }

        public bool IsDynamic
        {
            get { return _isDynamic; }
            set
            {
                _isDynamic = value;

                Reset();
            }
        }

        public bool AutoCloseWhenItemWithDropDownClicked
        {
            get { return _autoCloseWhenItemWithDropDownClicked; }
            set { _autoCloseWhenItemWithDropDownClicked = value; }
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            // Clear the items so that all the consumer needs to do is
            // is add the dynamic items
            if (_isDynamic)
                Items.Clear();

            base.OnOpening(e);
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            base.OnClosed(e);

            Reset();
        }

        protected override void OnItemClicked(ToolStripItemClickedEventArgs e)
        {
            try
            {
                base.OnItemClicked(e);
            }
            finally
            {
                if (AutoCloseWhenItemWithDropDownClicked)
                {
                    ToolStripDropDownItem item = e.ClickedItem as ToolStripDropDownItem;
                    if (item != null && item.DropDown.Visible)
                    {
                        Close(ToolStripDropDownCloseReason.ItemClicked);
                    }
                }
            }
        }

        private void Reset()
        {
            if (_isDynamic)
            {
                Items.Clear();
                Items.Add(new ToolStripMenuItem()); // Dummy item so that it will open first time
            }
        }
    }
}
