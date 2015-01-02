// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.UI.Presenters;

namespace AudioSwitcher.ApplicationModel.Startup
{
    // Displays a tray icon in the taskbar tray
    [Export(typeof(IStartupService))]
    internal class NotificationIconService : IStartupService, IDisposable
    {
        private AudioNotifyIcon _notifyIcon;
        private readonly AudioDeviceManager _deviceManager;
        private readonly CommandManager _commandManager;
        private readonly IApplication _application;

        [ImportingConstructor]
        public NotificationIconService(AudioDeviceManager deviceManager, CommandManager commandManager, IApplication application)
        {
            _deviceManager = deviceManager;
            _commandManager = commandManager;
            _application = application;
        }

        public void Startup()
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
