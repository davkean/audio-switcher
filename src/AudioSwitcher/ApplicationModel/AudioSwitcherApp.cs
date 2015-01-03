// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;

namespace AudioSwitcher.ApplicationModel
{
    // Represents the lifetime of the audio switcher application
    [Export(typeof(IApplication))]
    internal class AudioSwitcherApp : IApplication
    {
        private readonly Lazy<IStartupService, IPriorityMetadata>[] _startupServices;

        [ImportingConstructor]
        public AudioSwitcherApp([ImportMany]Lazy<IStartupService, IPriorityMetadata>[] startupServices)
        {
            _startupServices = startupServices.OrderBy(s => s.Metadata.Priority)
                                              .ToArray();
        }

        public event EventHandler Idle;

        public string Title
        {
            get { return Resources.Title; }
        }

        public Icon NotificationAreaIcon
        {
            get { return Resources.NotificationArea; }
        }

        public void Run()
        {
            foreach (var service in _startupServices)
            {
                if (!service.Value.Startup())
                    return;
            }

            Application.Idle += OnApplicationIdle;
            Application.Run();
        }

        public void Shutdown()
        {
            Application.Exit();
            Application.Idle -= OnApplicationIdle;
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            var handler = Idle;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
