// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.Presentation.UI.Interop;

namespace AudioSwitcher.Presentation
{
	// Provides the base class for context menu presenters
	internal abstract class ContextMenuPresenter : Presenter, IDisposable
	{
		private readonly AudioContextMenuStrip _contextMenu;
		private readonly IApplication _application;

		protected ContextMenuPresenter(IApplication application)
		{
			_application = application;
			_contextMenu = CreateContextMenu();
			_contextMenu.Closed += (sender, e) => OnClosed(EventArgs.Empty);
		}

		public AudioContextMenuStrip ContextMenu
		{
			get { return _contextMenu; }
		}

		protected virtual AudioContextMenuStrip CreateContextMenu()
		{
			return new AudioContextMenuStrip();
		}

		public void Show(Point screenLocation)
		{
			ContextMenu.ShowInSystemTray(screenLocation);
		}

		public void Close()
		{
			_contextMenu.Close(ToolStripDropDownCloseReason.AppFocusChange);
		}

		public virtual void Dispose()
		{
			// WORKAROUND: It's not possible to dispose of a context menu strip
			// in its Closed event because it tries to do some work after that
			// and throws ObjectDisposedException. To workaround this, we hold
			// off disposing them until the next idle event.
			_application.RunOnNextIdle(() => _contextMenu.Dispose());
		}
	}
}
