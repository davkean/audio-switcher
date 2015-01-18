// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;
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
        public NotificationIconContextMenuPresenter(IApplication application, CommandManager commandManager)
            : base(application)
        {
            _commandManager = commandManager;
        }

        protected override void Bind()
        {
            ToolStripDropDown settings = ContextMenu.Add(Resources.Settings).DropDown;
            settings.BindCommand(_commandManager, CommandId.RunAtWindowsStartup);
            settings.AddSeparator();
            settings.BindCommand(_commandManager, CommandId.AutomaticallySwitchToPluggedInDevice);

            ContextMenu.AddSeparator();

            ToolStripDropDown appearance = ContextMenu.Add(Resources.Appearance).DropDown;
            appearance.BindCommand(_commandManager, CommandId.ShowPlaybackDevices);
            appearance.BindCommand(_commandManager, CommandId.ShowRecordingDevices);
            appearance.AddSeparator();
            appearance.BindCommand(_commandManager, CommandId.ShowUnpluggedDevices);
            appearance.BindCommand(_commandManager, CommandId.ShowDisabledDevices);
            appearance.BindCommand(_commandManager, CommandId.ShowNotPresentDevices);

            ContextMenu.AddSeparator();
            ContextMenu.BindCommand(_commandManager, CommandId.Exit);
        }
    }
}
