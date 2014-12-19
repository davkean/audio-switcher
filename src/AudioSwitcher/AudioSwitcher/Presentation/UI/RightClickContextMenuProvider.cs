// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Presentation.CommandModel.Commands;
using AudioSwitcher.Presentation.UI;

namespace AudioSwitcher.Presentation.UI
{
    // Handles creating the context menu for the right-click menu
    internal class RightClickContextMenuProvider
    {
        public static AudioContextMenu CreateContextMenu(IApplication application)
        {
            AudioContextMenu context = new AudioContextMenu();
            context.DefaultDropDownDirection = ToolStripDropDownDirection.Left;

            ToolStripDropDown settingsContext = context.AddNestedItem(Resources.Settings);
            settingsContext.AddCommand(new RunAsStartupCommand());
            settingsContext.AddSeparator();
            settingsContext.AddCommand(new AutoSwitchToPluggedInDeviceCommand());

            context.AddSeparator();

            ToolStripDropDown showContext = context.AddNestedItem(Resources.Appearance);
            showContext.AddCommand(new ShowPlaybackDevicesCommand());
            showContext.AddCommand(new ShowRecordingDevicesCommand());
            showContext.AddSeparator();
            showContext.AddCommand(new ShowUnpluggedDevicesCommand());
            showContext.AddCommand(new ShowDisabledDevicesCommand());
            showContext.AddCommand(new ShowNotPresentDevices());

            context.AddSeparator();
            context.AddCommand(new ExitCommand(application));

            return context;
        }
    }
}
