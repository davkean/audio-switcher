// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using AudioSwitcher.Presentation;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.UI.Commands;

namespace AudioSwitcher.UI.Presenters
{
    // Presents the context menu when right-clicking on the notification icon
    [Presenter(PresenterId.NotificationAreaContextMenu)]
    internal class NotificationAreaContextMenuPresenter : ContextMenuPresenter
    {
        private readonly CommandManager _commandManager;

        [ImportingConstructor]
        public NotificationAreaContextMenuPresenter(CommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        protected override void Bind()
        {
            ToolStripDropDown settingsStrip = ContextMenu.AddNestedItem(Resources.Settings);
            settingsStrip.AddCommand(_commandManager, CommandId.ToggleRunAtWindowsStartup);
            settingsStrip.AddSeparator();
            settingsStrip.AddCommand(_commandManager, CommandId.ToggleAutomaticallySwitchToPluggedInDevice);

            ContextMenu.AddSeparator();

            ToolStripDropDown appearanceStrip = ContextMenu.AddNestedItem(Resources.Appearance);
            appearanceStrip.AddCommand(_commandManager, CommandId.ToggleShowPlaybackDevices);
            appearanceStrip.AddCommand(_commandManager, CommandId.ToggleShowRecordingDevices);
            appearanceStrip.AddSeparator();
            appearanceStrip.AddCommand(_commandManager, CommandId.ToggleShowUnpluggedDevices);
            appearanceStrip.AddCommand(_commandManager, CommandId.ToggleShowDisabledDevices);
            appearanceStrip.AddCommand(_commandManager, CommandId.ToggleShowNotPresentDevices);

            ContextMenu.AddSeparator();
            ContextMenu.AddCommand(_commandManager, CommandId.Exit);
        }
    }
}
