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

        void Start();

        void Shutdown();

        /// <summary>
        ///     Queues and runs the specified action on the next application idle.
        /// </summary>
        /// <param name="action">
        ///     The <see cref="Action"/> to run.
        /// </param>
        void RunOnNextIdle(Action action);
    }
}
