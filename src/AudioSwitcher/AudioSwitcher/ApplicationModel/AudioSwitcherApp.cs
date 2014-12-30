// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel.Startup;

namespace AudioSwitcher.ApplicationModel
{
    // Represents the lifetime of the audio switcher application
    [Export(typeof(IApplication))]
    internal class AudioSwitcherApp : IApplication, IDisposable
    {
        private readonly SingleInstanceApp _singleInstance;
        private readonly Lazy<IStartupService>[] _startupServices;

        [ImportingConstructor]
        public AudioSwitcherApp([ImportMany]Lazy<IStartupService>[] startupServices)
        {
            _startupServices = startupServices;
            _singleInstance =  new SingleInstanceApp();
        }

        public string Title
        {
            get { return Resources.Title; }
        }

        public Icon Icon
        {
            get { return Resources.HeadphonesTrayIcon; }
        }

        public void Run()
        {
            if (!_singleInstance.IsFirstInstance)
                return;

            foreach (var service in _startupServices)
            {
                service.Value.Startup();
            }

            Application.EnableVisualStyles();
            Application.Run();
        }

        public void Shutdown()
        {
            Application.Exit();            
        }

        public void Dispose()
        {
            _singleInstance.Dispose();
        }
    }
}
