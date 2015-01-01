// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Drawing;

namespace AudioSwitcher.ApplicationModel
{
    internal interface IApplication
    {
        string Title
        {
            get;
        }

        Icon NotificationAreaIcon
        {
            get;
        }

        void Run();

        void Shutdown();
    }
}
