// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

namespace AudioSwitcher.ApplicationModel.Settings
{
    internal interface ISettingService
    {
        bool GetBoolean(string name);

        string GetString(string name);
    }
}
