// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Presentation.CommandModel
{
    // Represents a command that takes an argument
    internal abstract class Command<T> : CommandBase
    {
        protected Command()
        {
        }

        public override sealed void Run(object argument)
        {
            if (!(argument is T))
                throw new ArgumentException();

            Run((T)argument);
        }

        public override sealed void UpdateStatus(object argument)
        {
            if (!(argument is T))
                throw new ArgumentException();

            UpdateStatus((T)argument);
        }

        public abstract void Run(T argument);

        public virtual void UpdateStatus(T argument)
        {
        }
    }
}
