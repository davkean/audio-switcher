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
