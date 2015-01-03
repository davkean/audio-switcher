// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Presentation;

namespace AudioSwitcher.UI.Presenters
{
    [Presenter(PresenterId.NotificationArea)]
    internal class NotificationAreaPresenter : NonModalPresenter, IDisposable
    {
        private readonly NotifyIcon _icon = new NotifyIcon();
        private readonly PresenterManager _presenterManager;

        [ImportingConstructor]
        public NotificationAreaPresenter(IApplication application, PresenterManager presenterManager)
        {
            _presenterManager = presenterManager;
            _icon = new NotifyIcon();
            _icon.Text = application.Title;
            _icon.Icon = application.NotificationAreaIcon;
            _icon.MouseUp += OnNotifyIconMouseUp;
        }

        private void OnNotifyIconMouseUp(object sender, MouseEventArgs e)
        {
            // NOTE: WinForm's NotifyIcon is opting into the legacy mechanism for retrieving mouse and keyboard 
            // messages. This means that ENTER, SPACE and MENU key all come through as mouse events. The shell 
            // even moves the pointer over the top of the icon when you press a key so that Cursor.Position 
            // returns the correct value.
            // 
            // Don't be tempted to use any other of the MouseEventArgs properties other than Button, they are bogus
            // and are not set to correct values.
            //
            // BUG #14: ENTER seems to be sending two MouseUp events, causing us to show and them immediately dismiss
            // the context menu.

            if (e.Button == MouseButtons.Left)
            {
                _presenterManager.ShowContextMenu(PresenterId.DeviceContextMenu, Cursor.Position);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _presenterManager.ShowContextMenu(PresenterId.NotificationAreaContextMenu, Cursor.Position);
            }
        }

        public override void ShowNonModal()
        {
            _icon.Visible = true;
        }

        public void Dispose()
        {
            _icon.Dispose();
        }
    }
}
