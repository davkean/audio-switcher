// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;

namespace AudioSwitcher.ApplicationModel.Startup
{
    // Displays a tray icon in the taskbar tray
    [Export(typeof(IStartupService))]
    internal class NotificationIconService : IStartupService, IDisposable
    {
        private AudioNotifyIcon _trayIcon;
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
            _trayIcon = new AudioNotifyIcon();
            _trayIcon.Title = _application.Title;
            _trayIcon.Icon = _application.Icon;
            _trayIcon.LeftClickContextMenuStrip = LeftClickContextMenuProvider.CreateContextMenu(_deviceManager);
            _trayIcon.RightClickContextMenuStrip = RightClickContextMenuProvider.CreateContextMenu(_commandManager);
        }

        public void Dispose()
        {
            _trayIcon.Dispose();
        }
    }
}
