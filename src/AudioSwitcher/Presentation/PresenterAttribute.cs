// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;

namespace AudioSwitcher.Presentation
{
    /// <summary>
    ///     Specifies that a class is a presenter.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class PresenterAttribute : ExportAttribute
    {
        private readonly string _id;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PresenterAttribute"/> with the specified unique command ID.
        /// </summary>
        /// <param name="id">
        ///     A <see cref="string"/> containing a unique ID of the <see cref="IPresenter"/>.
        /// </param>
        public PresenterAttribute(string id)
            : base(typeof(IPresenter))
        {
            _id = id;
        }

        public string Id
        {
            get { return _id;  }
        }

        public bool IsToggle
        {
            get;
            set;
        }
    }
}
