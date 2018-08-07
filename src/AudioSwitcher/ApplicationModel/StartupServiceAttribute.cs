// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;

namespace AudioSwitcher.ApplicationModel
{
    /// <summary>
    ///     Specifies that a class is a command.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class StartupServiceAttribute : ExportAttribute
    {
        public StartupServiceAttribute()
            : base(typeof(IStartupService))
        {
        }

        /// <summary>
        ///     Gets or sets the priority in which the <see cref="IService"/> object's <see cref="IStartup.Startup"/> method 
        /// </summary>
        /// <value>
        ///     An <see cref="int"/> containing the priority in which <see cref="IService"/> object's <see cref="IStartup.Startup"/> method is called. Lower is higher in priority.
        /// </value>
        public int Priority
        {
            get;
            set;
        }
    }
}
