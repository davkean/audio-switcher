// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.UI.Commands
{
    internal static class CommandId
    {
        public const string AutomaticallySwitchToPluggedInDevice =  "Settings.AutomaticallySwitchToPluggedInDevice";
        public const string RunAtWindowsStartup =                   "Settings.RunAtWindowsStartup";

        public const string ShowPlaybackDevices =                   "Appearance.ShowPlaybackDevices";
        public const string ShowRecordingDevices =                  "Appearance.ShowRecordingDevices";

        public const string ShowDisabledDevices =                   "Appearance.ShowDisabledDevices";
        public const string ShowNotPresentDevices =                 "Appearance.ShowNotPresentDevices";
        public const string ShowUnpluggedDevices =                  "Appearance.ShowUnpluggedDevices";

        public const string Exit =                                  "Application.Exit";

        public const string SetAsDefaultCommunicationDevice =       "Device.SetAsDefaultCommunicationDevice";
        public const string SetAsDefaultMultimediaDevice =          "Device.SetAsDefaultMultimediaDevice";
        public const string SetAsDefaultDevice =                    "Device.SetAsDefaultDevice";

        public const string NoDevices =                             "Device.NoDevices";
        public const string NoPlaybackDevices =                     "Device.NoPlaybackDevices";
        public const string NoRecordingDevices =                    "Device.NoRecordingDevices";
    }
}
