// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    internal class ShowUnpluggedDevicesCommand : Command
    {
        public ShowUnpluggedDevicesCommand()
        {
            Text = Resources.ShowUnpluggedDevices;
            Bitmap bitmap = Resources.Unplugged;
            bitmap.MakeTransparent();
            Image = bitmap;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.ShowUnpluggedDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowUnpluggedDevices = !Settings.Default.ShowUnpluggedDevices;
        }
    }
}
