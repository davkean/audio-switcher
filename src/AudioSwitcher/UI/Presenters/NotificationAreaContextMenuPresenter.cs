// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
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
            settingsStrip.AddCommand(_commandManager, CommandId.RunAtWindowsStartup);
            settingsStrip.AddSeparator();
            settingsStrip.AddCommand(_commandManager, CommandId.AutomaticallySwitchToPluggedInDevice);

            ContextMenu.AddSeparator();

            ToolStripDropDown appearanceStrip = ContextMenu.AddNestedItem(Resources.Appearance);
            appearanceStrip.AddCommand(_commandManager, CommandId.ShowPlaybackDevices);
            appearanceStrip.AddCommand(_commandManager, CommandId.ShowRecordingDevices);
            appearanceStrip.AddSeparator();
            appearanceStrip.AddCommand(_commandManager, CommandId.ShowUnpluggedDevices);
            appearanceStrip.AddCommand(_commandManager, CommandId.ShowDisabledDevices);
            appearanceStrip.AddCommand(_commandManager, CommandId.ShowNotPresentDevices);

            ContextMenu.AddSeparator();
            ContextMenu.AddCommand(_commandManager, CommandId.Exit);
        }
    }
}
