// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Forms;
using AudioSwitcher.ComponentModel;
using System.Collections.Generic;

namespace AudioSwitcher.ApplicationModel
{
    // Represents the lifetime of the audio switcher application
    [Export(typeof(IApplication))]
    internal class AudioSwitcherApp : IApplication
    {
        private readonly Lazy<IStartupService, IPriorityMetadata>[] _startupServices;
        private readonly Queue<Action> _idleActions = new Queue<Action>();

        [ImportingConstructor]
        public AudioSwitcherApp([ImportMany]Lazy<IStartupService, IPriorityMetadata>[] startupServices)
        {
            _startupServices = startupServices.OrderBy(s => s.Metadata.Priority)
                                              .ToArray();
        }

        public string Title
        {
            get { return Resources.Title; }
        }

        public Icon NotificationAreaIcon
        {
            get { return Resources.NotificationArea; }
        }

        public void Start()
        {
            // Some of the startup services expect, or require a SynchronizationContext, 
            // so we run them after the message loop has started.
            RunOnNextIdle(() => RunStartupServices());

            Application.Idle += OnApplicationIdle;
            Application.Run();
        }

        public void Shutdown()
        {
            Application.Exit();
            Application.Idle -= OnApplicationIdle;
        }

        public void RunOnNextIdle(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _idleActions.Enqueue(action);
        }

        private void RunStartupServices()
        {
            foreach (var service in _startupServices)
            {
                if (!service.Value.Startup())
                {
                    Shutdown();
                    break;
                }
            }
        }

        private void OnApplicationIdle(object sender, EventArgs e)
        {
            if (_idleActions.Count > 0)
            {
                // Snapshot actions and then clear existing in
                // case one of actions queue additional work
                Action[] actions = _idleActions.ToArray();
                _idleActions.Clear();

                foreach (Action action in actions)
                {
                    action();
                }
            }
        }
    }
}
