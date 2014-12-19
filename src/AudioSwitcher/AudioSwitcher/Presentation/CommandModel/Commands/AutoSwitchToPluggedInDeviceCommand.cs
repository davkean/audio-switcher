// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Security;
using Microsoft.Win32;
using AudioSwitcher.Win32;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    internal class AutoSwitchToPluggedInDeviceCommand : Command
    {
        public AutoSwitchToPluggedInDeviceCommand()
        {
            Text = Resources.AutoSwitchToPluggedInDevice;
        }

        public override void UpdateStatus()
        {
            IsChecked = Settings.Default.AutoSwitchToPluggedInDevice;
        }

        public override void Run()
        {
            Settings.Default.AutoSwitchToPluggedInDevice = !Settings.Default.AutoSwitchToPluggedInDevice;
        }
    }
}
