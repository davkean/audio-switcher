// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.UI.Commands
{
    internal static class CommandId
    {
        public const string ToggleAutomaticallySwitchToPluggedInDevice  = "Settings.ToggleAutomaticallySwitchToPluggedInDevice";
        public const string ToggleRunAtWindowsStartup                   = "Settings.ToggleRunAtWindowsStartup";

        public const string ToggleShowPlaybackDevices                   = "Appearance.ToggleShowPlaybackDevices";
        public const string ToggleShowRecordingDevices                  = "Appearance.ToggleShowRecordingDevices";
        
        public const string ToggleShowDisabledDevices                   = "Appearance.ToggleShowDisabledDevices";
        public const string ToggleShowNotPresentDevices                 = "Appearance.ToggleShowNotPresentDevices";
        public const string ToggleShowUnpluggedDevices                  = "Appearance.ToggleShowUnpluggedDevices";

        public const string Exit                                        = "Application.Exit";

        public const string SetAsDefaultCommunicationDevice             = "Device.SetAsDefaultCommunicationDevice";
        public const string SetAsDefaultMultimediaDevice                = "Device.SetAsDefaultMultimediaDevice";
        public const string SetAsDefaultDevice                          = "Device.SetAsDefaultDevice";
        public const string NoDevices                                   = "Device.NoDevices";
        public const string NoRecordingDevices                          = "Device.NoRecordingDevices";
        public const string NoPlaybackDevices                           = "Device.NoPlaybackDevices";
    }
}
