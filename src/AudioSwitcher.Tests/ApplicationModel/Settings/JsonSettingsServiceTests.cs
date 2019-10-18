// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.IO;
using Xunit;

namespace AudioSwitcher.ApplicationModel.Settings
{
    [Trait("", "UnitTests")]
    public class JsonSettingsServiceTests
    {
        private readonly string _json =
        @"{
          \""ShowDisabledDevices\"": false,
          \""ShowUnpluggedDevices\"": false,
          \""ShowRecordingDevices\"": true,
          \""ShowPlaybackDevices\"": true,
          \""ShowNotPresentDevices\"": false,
          \""AutoSwitchToPluggedInDevice\"": false
        }";


        [Fact]
        public void GetBooleanValue_ReadsSettingFromJson()
        {
            var service = new JsonSettingsService(new StringReader(_json));

            var result = service.GetBoolean("ShowDisabledDevices");

            Assert.False(result);
        }
    }
}
