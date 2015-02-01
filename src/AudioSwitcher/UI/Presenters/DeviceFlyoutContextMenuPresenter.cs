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
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Presenters
{
    // Presents the context menu when right-clicking on the device flyout
    [Presenter(PresenterId.DeviceFlyoutContextMenu)]
    internal class DeviceFlyoutContextMenuPresenter : ContextMenuPresenter
    {
        private readonly CommandManager _commandManager;

        [ImportingConstructor]
        public DeviceFlyoutContextMenuPresenter(IApplication application, CommandManager commandManager)
            : base(application)
        {
            _commandManager = commandManager;
        }

        protected override AudioContextMenuStrip CreateContextMenu()
        {
            AudioContextMenuStrip strip = base.CreateContextMenu();
            strip.DefaultDropDownDirection = ToolStripDropDownDirection.Right;
            strip.WorkingAreaConstrained = true;

            return strip;
        }

        public override void Bind(object argument)
        {
            AudioDeviceViewModel device = (AudioDeviceViewModel)argument;

            ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, device);
            ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, device);
        }
    }
}
