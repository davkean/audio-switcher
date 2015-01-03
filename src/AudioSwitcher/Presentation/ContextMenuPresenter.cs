// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.Presentation.UI.Interop;

namespace AudioSwitcher.Presentation
{
    // Provides the base class for context menu presenters
    internal abstract class ContextMenuPresenter : Presenter, IDisposable
    {
        private readonly AudioContextMenuStrip _contextMenu;

        protected ContextMenuPresenter()
        {
            _contextMenu = new AudioContextMenuStrip();
            _contextMenu.Closed += (sender, e) => OnClosed(EventArgs.Empty);
        }

        public AudioContextMenuStrip ContextMenu
        {
            get { return _contextMenu; }
        }

        protected abstract void Bind();

        public void Show(Point screenLocation)
        {
            Bind();

            ContextMenu.ShowInSystemTray(screenLocation);
        }

        public void Close()
        {
            _contextMenu.Close(ToolStripDropDownCloseReason.AppFocusChange);
        }

        public void Dispose()
        {
            _contextMenu.Dispose();
        }
    }
}
