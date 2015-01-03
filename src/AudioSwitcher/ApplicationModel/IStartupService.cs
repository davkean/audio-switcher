// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.ApplicationModel
{
    // Indicates that a service that runs at application startup
    internal interface IStartupService
    {
        bool Startup();
    }
}
