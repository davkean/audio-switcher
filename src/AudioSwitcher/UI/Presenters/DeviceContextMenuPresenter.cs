// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Presentation.UI;
using AudioSwitcher.Presentation.UI.Renderer;
using AudioSwitcher.UI.Commands;
using AudioSwitcher.UI.ViewModels;

namespace AudioSwitcher.UI.Presenters
{
    // Presents the device context menu when left-clicking on the notification icon
    [Presenter(PresenterId.DeviceContextMenu, IsToggle=true)]
    internal class DeviceContextMenuPresenter : ContextMenuPresenter, IDisposable
    {
        private readonly AudioDeviceViewModelManager _viewModelManager;
        private readonly CommandManager _commandManager;

        [ImportingConstructor]
        public DeviceContextMenuPresenter(AudioDeviceViewModelManager viewModelManager, CommandManager commandManager)
        {
            _viewModelManager = viewModelManager;
            _viewModelManager.Changed += OnViewModelsChanged;
            _commandManager = commandManager;
        }

        public override void Dispose()
        {
            base.Dispose();

            _viewModelManager.Changed -= OnViewModelsChanged;
        }

        private void OnViewModelsChanged(object sender, System.EventArgs e)
        {
            ContextMenu.RefreshCommands();
        }

        protected override void Bind()
        {
            ContextMenu.Renderer = new DeviceToolStripNativeRender(ToolbarTheme.Toolbar);
            ContextMenu.AutoCloseWhenItemWithDropDownClicked = true; // When something clicks the "Device" we auto close 
            ContextMenu.WorkingAreaConstrained = true;

            AddDeviceCommands(AudioDeviceKind.Playback, CommandId.NoPlaybackDevices);
            AddDeviceCommands(AudioDeviceKind.Recording, CommandId.NoRecordingDevices);

            ContextMenu.BindCommand(_commandManager, CommandId.NoDevices);
        }

        private void AddDeviceCommands(AudioDeviceKind kind, string noDeviceCommandId)
        {
            ContextMenu.AddSeparatorIfNeeded();

            AudioDeviceViewModel[] devices = GetDevices(kind);
            AddDeviceCommands(devices);

            ContextMenu.BindCommand(_commandManager, noDeviceCommandId);
        }

        private void AddDeviceCommands(AudioDeviceViewModel[] devices)
        {
            foreach (AudioDeviceViewModel device in devices)
            {
                ToolStripMenuItem menu = ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultDevice, device);
                menu.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, device);
                menu.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultCommunicationDevice, device);
            }
        }

        private AudioDeviceViewModel[] GetDevices(AudioDeviceKind kind)
        {
            return _viewModelManager.ViewModels.Where(v => v.Device.Kind == kind)
                                               .ToArray();
        }
    }
}
