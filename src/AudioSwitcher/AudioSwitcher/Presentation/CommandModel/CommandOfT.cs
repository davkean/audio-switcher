// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Presentation.CommandModel
{
    // Represents a command that takes an argument
    internal abstract class Command<T> : Command, ICommand
    {
        protected Command()
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
