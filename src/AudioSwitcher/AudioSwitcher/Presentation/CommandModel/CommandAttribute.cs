// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.Presentation.CommandModel
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class CommandAttribute : ExportAttribute
    {
        private readonly string _id;
        private bool _isDynamic;

        public CommandAttribute(string id)
            : base(typeof(ICommand))
        {
            _id = id;
        }

        public string Id
        {
            get { return _id;  }
        }

        public bool IsDynamic
        {
            get { return _isDynamic; }
            set { _isDynamic = value; }
        }
    }
}
