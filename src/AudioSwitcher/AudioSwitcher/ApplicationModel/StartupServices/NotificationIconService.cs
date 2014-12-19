// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.UI;

namespace AudioSwitcher.ApplicationModel.Startup
{
    // Displays a tray icon in the taskbar tray
    [Export(typeof(IStartupService))]
    internal class NotificationIconService : IStartupService, IDisposable
    {
        private AudioNotifyIcon _trayIcon;
        private readonly AudioDeviceManager _manager;
        private readonly IApplication _application;

        [ImportingConstructor]
        public NotificationIconService(AudioDeviceManager manager, IApplication application)
        {
            _manager = manager;
            _application = application;
        }

        public void Startup()
        {
            _trayIcon = new AudioNotifyIcon();
            _trayIcon.Title = _application.Title;
            _trayIcon.Icon = _application.Icon;
            _trayIcon.LeftClickContextMenuStrip = LeftClickContextMenuProvider.CreateContextMenu(_manager);
            _trayIcon.RightClickContextMenuStrip = RightClickContextMenuProvider.CreateContextMenu(_application);
        }

        public void Dispose()
        {
            _trayIcon.Dispose();
        }
    }
}
