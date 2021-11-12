// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
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

        void Start();

        void Shutdown();

        /// <summary>
        ///     Queues and runs the specified action on the next application idle.
        /// </summary>
        /// <param name="action">
        ///     The <see cref="Action"/> to run.
        /// </param>
        void RunOnNextIdle(Action action);

        /// <summary>
        ///     Gets the executable path of the application.
        /// </summary>
        /// <value>
        ///     The executable path of the application.
        /// </value>
        string ExecutablePath
        {
            get;
        }
        
    }
}
