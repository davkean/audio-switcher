// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;

namespace AudioSwitcher.ApplicationModel
{
    [StartupService]
    internal class SaveSettingsOnShutdownService : IStartupService, IDisposable
    {
        [ImportingConstructor]
        public SaveSettingsOnShutdownService()
        {
        }

        public bool Startup()
        {
            return true;
        }

        public void Dispose()
        {
            Settings.Default.Save();
        }
    }
}
