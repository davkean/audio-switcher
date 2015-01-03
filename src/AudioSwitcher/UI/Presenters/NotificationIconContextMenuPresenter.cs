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
    [Presenter(PresenterId.NotificationIconContextMenu)]
    internal class NotificationIconContextMenuPresenter : ContextMenuPresenter
    {
        private readonly CommandManager _commandManager;

        [ImportingConstructor]
        public NotificationIconContextMenuPresenter(CommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        protected override void Bind()
        {
            ToolStripMenuItem settings = ContextMenu.Add(Resources.Settings);
            settings.DropDown.BindCommand(_commandManager, CommandId.RunAtWindowsStartup);
            settings.DropDown.AddSeparator();
            settings.DropDown.BindCommand(_commandManager, CommandId.AutomaticallySwitchToPluggedInDevice);

            ContextMenu.AddSeparator();

            ToolStripMenuItem appearance = ContextMenu.Add(Resources.Appearance);
            appearance.DropDown.BindCommand(_commandManager, CommandId.ShowPlaybackDevices);
            appearance.DropDown.BindCommand(_commandManager, CommandId.ShowRecordingDevices);
            appearance.DropDown.AddSeparator();
            appearance.DropDown.BindCommand(_commandManager, CommandId.ShowUnpluggedDevices);
            appearance.DropDown.BindCommand(_commandManager, CommandId.ShowDisabledDevices);
            appearance.DropDown.BindCommand(_commandManager, CommandId.ShowNotPresentDevices);

            ContextMenu.AddSeparator();
            ContextMenu.BindCommand(_commandManager, CommandId.Exit);
        }
    }
}
