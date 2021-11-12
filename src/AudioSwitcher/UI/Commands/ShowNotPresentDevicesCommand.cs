// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;

using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ShowNotPresentDevices)]
    internal class ShowNotPresentDevicesCommand : Command
    {
        [ImportingConstructor]
        public ShowNotPresentDevicesCommand()
        {
            Text = Resources.ShowNotPresentDevices;
        }

        public override void Refresh()
        {
            IsChecked = Settings.Default.ShowNotPresentDevices;
        }

        public override void Run()
        {
            Settings.Default.ShowNotPresentDevices = !Settings.Default.ShowNotPresentDevices;
        }
    }
}
