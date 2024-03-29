﻿// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

namespace AudioSwitcher.Presentation.CommandModel
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
