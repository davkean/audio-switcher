// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.ApplicationModel.Commands
{
    internal class DisabledCommand : Command
    {
        public DisabledCommand(string text)
            : base(text)
        {
            IsEnabled = false;
        }

        public override void Run()
        {   
        }
    }
}
