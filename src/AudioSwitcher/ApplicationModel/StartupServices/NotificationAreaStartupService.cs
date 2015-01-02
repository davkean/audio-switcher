// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;
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
            _presenterManager.ShowNonModal(PresenterId.NotificationArea);
        }
    }
}
