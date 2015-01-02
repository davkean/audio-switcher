// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;

namespace AudioSwitcher.UI.Presenters
{
    [Export(typeof(NotificationIconPresenter))]
    internal class NotificationIconPresenter : IDisposable
    {
        private AudioNotifyIcon _notifyIcon;
        private readonly AudioDeviceManager _deviceManager;
        private readonly CommandManager _commandManager;
        private readonly IApplication _application;

        [ImportingConstructor]
        public NotificationIconPresenter(IApplication application, AudioDeviceManager deviceManager, CommandManager commandManager)
        {
            _application = application;
            _deviceManager = deviceManager;
            _commandManager = commandManager;
        }

        public void Show()
        {
            _notifyIcon = new AudioNotifyIcon();
            _notifyIcon.Title = _application.Title;
            _notifyIcon.Icon = _application.NotificationAreaIcon;
            _notifyIcon.LeftClickContextMenuStrip = DeviceContextMenuPresenter.CreateContextMenu(_deviceManager, _commandManager);
            _notifyIcon.RightClickContextMenuStrip = RightClickContextMenuPresenter.CreateContextMenu(_commandManager);
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
        }
    }
}
