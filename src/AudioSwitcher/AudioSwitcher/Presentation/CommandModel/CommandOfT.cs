// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Presentation.CommandModel
{
    /// <summary>
    ///     Provides the base <see langword="abstract"/> class for commands that take an argument.
    /// </summary>
    internal abstract class Command<T> : Command, ICommand
    {
        protected Command()
            : this((string)null)
        {
        }

        protected Command(string text)
            : base(text)
        {   
        }

        public override sealed void Run()
        {
            throw new InvalidOperationException();
        }

        public override sealed void UpdateStatus()
        {
            throw new InvalidOperationException();
        }

        public abstract void Run(T argument);

        public virtual void UpdateStatus(T argument)
        {
        }


        void ICommand.Run(object argument)
        {
            if (!(argument is T))
                throw new ArgumentException();

            Run((T)argument);
        }

        void ICommand.UpdateStatus(object argument)
        {
            if (!(argument is T))
                throw new ArgumentException();

            UpdateStatus((T)argument);
        }
    }
}
