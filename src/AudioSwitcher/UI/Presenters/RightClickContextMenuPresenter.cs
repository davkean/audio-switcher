// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.UI.Commands;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;

namespace AudioSwitcher.UI.Presenters
{
    // Handles creating the context menu for the right-click menu
    internal class RightClickContextMenuPresenter
    {
        public static AudioContextMenu CreateContextMenu(CommandManager commandManager)
        {
            AudioContextMenu context = new AudioContextMenu();

            ToolStripDropDown settingsContext = context.AddNestedItem(Resources.Settings);
            settingsContext.AddCommand(commandManager, CommandId.ToggleRunAtWindowsStartup);
            settingsContext.AddSeparator();
            settingsContext.AddCommand(commandManager, CommandId.ToggleAutomaticallySwitchToPluggedInDevice);

            context.AddSeparator();

            ToolStripDropDown showContext = context.AddNestedItem(Resources.Appearance);
            showContext.AddCommand(commandManager, CommandId.ToggleShowPlaybackDevices);
            showContext.AddCommand(commandManager, CommandId.ToggleShowRecordingDevices);
            showContext.AddSeparator();
            showContext.AddCommand(commandManager, CommandId.ToggleShowUnpluggedDevices);
            showContext.AddCommand(commandManager, CommandId.ToggleShowDisabledDevices);
            showContext.AddCommand(commandManager, CommandId.ToggleShowNotPresentDevices);

            context.AddSeparator();
            context.AddCommand(commandManager, CommandId.Exit);

            return context;
        }
    }
}
