// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal class ShowDisabledDevicesCommand : Command
    {
        public ShowDisabledDevicesCommand()
        {
            Text = Resources.ShowDisabledDevices;

            Bitmap bitmap = Resources.Disabled;
            bitmap.MakeTransparent();
            Image = bitmap;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowDisabledDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowDisabledDevices = !Settings.Default.ShowDisabledDevices;
        }
    }
}
