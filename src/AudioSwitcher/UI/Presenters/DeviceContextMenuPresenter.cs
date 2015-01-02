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

            AddDeviceCommands(AudioDeviceKind.Playback, Settings.Default.ShowPlaybackDevices, CommandId.NoPlaybackDevices);
            AddDeviceCommands(AudioDeviceKind.Recording, Settings.Default.ShowRecordingDevices, CommandId.NoRecordingDevices);

            if (ContextMenu.Items.Count == 0)
                ContextMenu.AddCommand(_commandManager, CommandId.NoDevices);
        }

        private void AddDeviceCommands(AudioDeviceKind kind, bool condition, string noDeviceCommandId)
        {
            if (condition)
            {
                ContextMenu.AddSeparatorIfNeeded();

                AudioDeviceCollection devices = GetDevices(kind);
                if (devices.Count == 0)
                {
                    ContextMenu.AddCommand(_commandManager, noDeviceCommandId);
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
                Func<object> argumentGetter = () => device;

                ToolStripDropDown dropDown = ContextMenu.AddNestedCommand(_commandManager, CommandId.SetAsDefaultDevice, argumentGetter);
                dropDown.AddCommand(_commandManager, CommandId.SetAsDefaultMultimediaDevice, argumentGetter);
                dropDown.AddCommand(_commandManager, CommandId.SetAsDefaultCommunicationDevice, argumentGetter);
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
