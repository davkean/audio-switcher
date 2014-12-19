// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    internal class ShowNotPresentDevices : Command
    {
        public ShowNotPresentDevices()
        {
            Text = Resources.ShowNotPresentDevices;
            Image = Resources.NotPresent.ToBitmap();
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowNotPresentDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowNotPresentDevices = !Settings.Default.ShowNotPresentDevices;
        }
    }
}
