// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    internal class DisabledCommand : Command
    {
        public DisabledCommand(string text)
        {
            IsEnabled = false;
            Text = text;
        }

        public override void Run()
        {   
        }
    }
}
