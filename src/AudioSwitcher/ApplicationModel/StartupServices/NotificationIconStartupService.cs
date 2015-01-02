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
    [Export(typeof(IStartupService))]
    internal class NotificationIconStartupService : IStartupService, IDisposable
    {
        private readonly ExportFactory<NotificationIconPresenter> _factory;
        private ExportLifetimeContext<NotificationIconPresenter> _lifetime;

        [ImportingConstructor]
        public NotificationIconStartupService(ExportFactory<NotificationIconPresenter> factory)
        {
            _factory = factory;
        }

        public void Startup()
        {
            _lifetime = _factory.CreateExport();
            _lifetime.Value.Show();
        }

        public void Dispose()
        {
            _lifetime.Dispose();
        }
    }
}
