// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.ComponentModel.Composition;
using System.Windows.Forms;
using AudioSwitcher.Presentation;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.UI.Commands;
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Presenters
{
    // Presents the context menu when right-clicking on the notification icon
    [Presenter(PresenterId.DeviceContextMenu)]
    internal class DeviceContextMenuPresenter : ContextMenuPresenter
    {
        private readonly CommandManager _commandManager;

        [ImportingConstructor]
        public DeviceContextMenuPresenter(CommandManager commandManager)
        {
            _commandManager = commandManager;
        }

        public AudioDeviceViewModel Device
        {
            get;
            set;
        }

        protected override void Bind()
        {
           ContextMenu.DefaultDropDownDirection = ToolStripDropDownDirection.Right;
           ContextMenu.WorkingAreaConstrained = true;
           ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, Device);
           ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, Device);
        }
    }
}
