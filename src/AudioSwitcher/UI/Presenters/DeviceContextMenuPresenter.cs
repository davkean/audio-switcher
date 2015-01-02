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
using AudioSwitcher.UI.Commands;

namespace AudioSwitcher.UI.Presenters
{
    // Presents the device context menu when left-clicking on the notification icon
    [Presenter(PresenterId.DeviceContextMenu, IsToggle=true)]
    internal class DeviceContextMenuPresenter : ContextMenuPresenter
    {
        private readonly AudioDeviceManager _deviceManager;
        private readonly CommandManager _commandManager;

        [ImportingConstructor]
        public DeviceContextMenuPresenter(AudioDeviceManager deviceManager, CommandManager commandManager)
        {
            _deviceManager = deviceManager;
            _commandManager = commandManager;
        }

        protected override void Bind()
        {
            ContextMenu.AutoCloseWhenItemWithDropDownClicked = true; // When something clicks the "Device" we auto close 
            ContextMenu.WorkingAreaConstrained = true;

            AddDeviceCommands(AudioDeviceKind.Playback, Settings.Default.ShowPlaybackDevices, Resources.NoPlaybackDevices);
            AddDeviceCommands(AudioDeviceKind.Recording, Settings.Default.ShowRecordingDevices, Resources.NoRecordingDevices);

            if (ContextMenu.Items.Count == 0)
                ContextMenu.AddDisabled(Resources.NoDevices);
        }

        private void AddDeviceCommands(AudioDeviceKind kind, bool condition, string noDeviceText)
        {
            if (condition)
            {
                ContextMenu.AddSeparatorIfNeeded();

                AudioDeviceCollection devices = GetDevices(kind);
                if (devices.Count == 0)
                {
                    ContextMenu.AddDisabled(noDeviceText);
                }
                else
                {
                    AddCommand(devices, AudioDeviceState.Active);
                    AddCommand(devices, AudioDeviceState.Unplugged);
                    AddCommand(devices, AudioDeviceState.Disabled);
                    AddCommand(devices, AudioDeviceState.NotPresent);
                }
            }
        }

        private void AddCommand(AudioDeviceCollection devices, AudioDeviceState state)
        {
            foreach (AudioDevice device in devices.Where(d => d.State == state))
            {
                ToolStripMenuItem menu = ContextMenu.BindCommand(_commandManager, CommandId.SetAsDefaultDevice, device);
                menu.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, device);
                menu.DropDown.BindCommand(_commandManager, CommandId.SetAsDefaultCommunicationDevice, device);
            }
        }

        private AudioDeviceCollection GetDevices(AudioDeviceKind kind)
        {
            AudioDeviceState state = AudioDeviceState.Active;
            if (Settings.Default.ShowDisabledDevices)
            {
                state |= AudioDeviceState.Disabled;
            }

            if (Settings.Default.ShowUnpluggedDevices)
            {
                state |= AudioDeviceState.Unplugged;
            }

            if (Settings.Default.ShowNotPresentDevices)
            {
                state |= AudioDeviceState.NotPresent;
            }

            return _deviceManager.GetAudioDevices(kind, state);
        }
    }
}
