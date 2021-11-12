// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.AutomaticallySwitchToPluggedInDevice)]
    internal class AutomaticallySwitchToPluggedInDeviceCommand : Command
    {
        public AutomaticallySwitchToPluggedInDeviceCommand()
        {
            Text = Resources.AutomaticallySwitchToPluggedInDevice;
        }

        public override void Refresh()
        {
            IsChecked = Settings.Default.AutoSwitchToPluggedInDevice;
        }

        public override void Run()
        {
            Settings.Default.AutoSwitchToPluggedInDevice = !Settings.Default.AutoSwitchToPluggedInDevice;
        }
    }
}
