// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel.Commands;
using AudioSwitcher.Audio;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.UI
{
    // Handles creating the context menu for the device menu
    internal class LeftClickContextMenuProvider
    {
        public static AudioContextMenu CreateContextMenu(AudioDeviceManager deviceManager, CommandManager commandManager)
        {
            AudioContextMenu strip = new AudioContextMenu();

            strip.IsDynamic = true; // We dynamically add the devices when the menu is opened
            strip.AutoCloseWhenItemWithDropDownClicked = true; // When something clicks the "Device" we autoclose 
            strip.ShowImageMargin = true;
            strip.Opening += (sender, e) => OnContextMenuOpening(deviceManager, commandManager, strip);

            return strip;
        }

        private static void OnContextMenuOpening(AudioDeviceManager deviceManager, CommandManager commandManager, AudioContextMenu strip)
        {
            AddDeviceCommands(deviceManager, commandManager, strip, AudioDeviceKind.Playback, Settings.Default.ShowPlaybackDevices, CommandId.NoPlaybackDevices);
            AddDeviceCommands(deviceManager, commandManager, strip, AudioDeviceKind.Recording, Settings.Default.ShowRecordingDevices, CommandId.NoRecordingDevices);

            if (strip.Items.Count == 0)
                strip.AddCommand(commandManager, CommandId.NoDevices);
        }

        private static void AddDeviceCommands(AudioDeviceManager deviceManager, CommandManager commandManager, ContextMenuStrip strip, AudioDeviceKind kind, bool condition, string noDeviceCommandId)
        {
            if (condition)
            {
                strip.AddSeparatorIfNeeded();

                AudioDeviceCollection devices = GetDevices(deviceManager, kind);
                if (devices.Count == 0)
                {
                    strip.AddCommand(commandManager, noDeviceCommandId);
                }
                else
                {
                    AddCommand(deviceManager, commandManager, strip, devices, AudioDeviceState.Active);
                    AddCommand(deviceManager, commandManager, strip, devices, AudioDeviceState.Unplugged);
                    AddCommand(deviceManager, commandManager, strip, devices, AudioDeviceState.Disabled);
                    AddCommand(deviceManager, commandManager, strip, devices, AudioDeviceState.NotPresent);
                }
            }
        }

        private static void AddCommand(AudioDeviceManager deviceManager, CommandManager commandManager, ContextMenuStrip strip, AudioDeviceCollection devices, AudioDeviceState state)
        {
            foreach (AudioDevice device in devices.Where(d => d.State == state))
            {
                Func<object> argumentGetter = () => device;

                ToolStripDropDown dropDown = strip.AddNestedCommand(commandManager, CommandId.SetAsDefaultDevice, argumentGetter);
                dropDown.AddCommand(commandManager, CommandId.SetAsDefaultMultimediaDevice, argumentGetter);
                dropDown.AddCommand(commandManager, CommandId.SetAsDefaultCommunicationDevice, argumentGetter);
            }
        }

        private static AudioDeviceCollection GetDevices(AudioDeviceManager manager, AudioDeviceKind kind)
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

            return manager.GetAudioDevices(kind, state);
        }
    }
}
