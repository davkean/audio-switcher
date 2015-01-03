// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.ApplicationModel
{
    // Indicates that a service that runs at application startup
    internal interface IStartupService
    {
        /// <summary>
        ///     Called to indicate that the application has started.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the application can start; otherwise, <see langword="false"/>.
        /// </returns>
        bool Startup();
    }
}
