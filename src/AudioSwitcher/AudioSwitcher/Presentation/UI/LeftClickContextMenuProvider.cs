// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System.Linq;
using System.Windows.Forms;
using AudioSwitcher.Audio;
using AudioSwitcher.ApplicationModel.Commands;

namespace AudioSwitcher.Presentation.UI
{
    // Handles creating the context menu for the device menu
    internal class LeftClickContextMenuProvider
    {
        public static AudioContextMenu CreateContextMenu(AudioDeviceManager manager)
        {
            AudioContextMenu strip = new AudioContextMenu();

            strip.IsDynamic = true; // We dynamically add the devices when the menu is opened
            strip.AutoCloseWhenItemWithDropDownClicked = true; // When something clicks the "Device" we autoclose 
            strip.ShowImageMargin = true;
            strip.Opening += (sender, e) => OnContextMenuOpening(manager, strip);

            return strip;
        }

        private static void OnContextMenuOpening(AudioDeviceManager manager, AudioContextMenu strip)
        {
            AddDeviceCommands(manager, strip, AudioDeviceKind.Playback, Settings.Default.ShowPlaybackDevices, Resources.NoPlaybackDevices);
            AddDeviceCommands(manager, strip, AudioDeviceKind.Recording, Settings.Default.ShowRecordingDevices, Resources.NoRecordingDevices);

            if (strip.Items.Count == 0)
                strip.AddCommand(new DisabledCommand(Resources.NoDevices));
        }

        private static void AddDeviceCommands(AudioDeviceManager manager, ContextMenuStrip strip, AudioDeviceKind kind, bool condition, string noDeviceText)
        {
            if (condition)
            {
                strip.AddSeparatorIfNeeded();

                AudioDeviceCollection devices = GetDevices(manager, kind);
                if (devices.Count == 0)
                {
                    strip.AddCommand(new DisabledCommand(noDeviceText));
                }
                else
                {
                    AddCommand(manager, strip, devices, AudioDeviceState.Active);
                    AddCommand(manager, strip, devices, AudioDeviceState.Unplugged);
                    AddCommand(manager, strip, devices, AudioDeviceState.Disabled);
                    AddCommand(manager, strip, devices, AudioDeviceState.NotPresent);
                }
            }
        }

        private static void AddCommand(AudioDeviceManager manager, ContextMenuStrip strip, AudioDeviceCollection devices, AudioDeviceState state)
        {
            foreach (AudioDevice device in devices.Where(d => d.State == state))
            {
                ToolStripDropDown dropDown = strip.AddNestedCommand(new AudioDeviceCommand(manager, device));
                                
                if (device.State == AudioDeviceState.Active)
                {
                    dropDown.AddCommand(new SetAsDefaultMultimediaDeviceCommand(manager, device));
                    dropDown.AddCommand(new SetAsDefaultCommunicationDeviceCommand(manager, device));
                }
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
