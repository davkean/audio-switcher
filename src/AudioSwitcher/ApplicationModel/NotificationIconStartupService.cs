// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;
using AudioSwitcher.Presentation;
using AudioSwitcher.UI.Presenters;

namespace AudioSwitcher.ApplicationModel
{
    [StartupService]
    internal class NotificationIconStartupService : IStartupService
    {
        private readonly PresenterHost _presenterHost;

        [ImportingConstructor]
        public NotificationIconStartupService(PresenterHost presenterHost)
        {
            _presenterHost = presenterHost;
        }

        public bool Run()
        {
            _presenterHost.Show(PresenterId.NotificationIcon);
            return true;
        }
    }
}
