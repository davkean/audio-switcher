// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace AudioSwitcher.ApplicationModel
{
    [StartupService]
    internal class SaveSettingsService : IStartupService, IDisposable
    {
        [ImportingConstructor]
        public SaveSettingsService()
        {
        }

        public bool Startup()
        {
            //Settings.Default.PropertyChanged += OnSettingsPropertyChanged;
            return true;
        }

        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //Settings.Default.Save();
        }

        public void Dispose()
        {
            //Settings.Default.PropertyChanged -= OnSettingsPropertyChanged;
        }
    }
}
