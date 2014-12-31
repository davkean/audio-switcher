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
        {
            IsEnabled = false;
            Text = text;
        }

        public override void Run()
        {   
        }
    }
}
