// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.ApplicationModel.Startup
{
    // Indicates a service that runs at application startup
    internal interface IStartupService
    {
        void Startup();
    }
}
