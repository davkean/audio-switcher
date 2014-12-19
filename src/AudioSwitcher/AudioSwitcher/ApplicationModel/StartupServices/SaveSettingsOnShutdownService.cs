// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;

namespace AudioSwitcher.ApplicationModel.Startup
{
    [Export(typeof(IStartupService))]
    internal class SaveSettingsOnShutdownService : IStartupService, IDisposable
    {
        [ImportingConstructor]
        public SaveSettingsOnShutdownService()
        {
        }

        public void Startup()
        {
        }

        public void Dispose()
        {
            Settings.Default.Save();
        }
    }
}
