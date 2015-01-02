// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.CommandModel
{
    /// <summary>
    ///     Specifies that a class is a command.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class CommandAttribute : ExportAttribute
    {
        private readonly string _id;
        private bool _isReusable = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CommandAttribute"/> with the specified unique command ID.
        /// </summary>
        /// <param name="id">
        ///     A <see cref="string"/> containing a unique ID of the <see cref="Command"/>.
        /// </param>
        public CommandAttribute(string id)
            : base(typeof(ICommand))
        {
            _id = id;
        }

        public string Id
        {
            get { return _id;  }
        }

        public bool IsReusable
        {
            get { return _isReusable; }
            set { _isReusable = value; }
        }
    }
}
