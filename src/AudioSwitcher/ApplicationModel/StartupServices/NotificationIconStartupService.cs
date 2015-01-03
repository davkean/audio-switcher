// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation;
using AudioSwitcher.UI.Presenters;

namespace AudioSwitcher.ApplicationModel.Startup
{
    [Export(typeof(IStartupService))]
    internal class NotificationIconStartupService : IStartupService
    {
        private readonly PresenterManager _presenterManager;

        [ImportingConstructor]
        public NotificationIconStartupService(PresenterManager presenterManager)
        {
            _presenterManager = presenterManager;
        }

        public void Startup()
        {
            _presenterManager.ShowNonModal(PresenterId.NotificationIcon);
        }
    }
}
