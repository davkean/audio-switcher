// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.Windows.Forms;
using AudioSwitcher.ApplicationModel;

namespace AudioSwitcher.Presentation.CommandModel.Commands
{
    internal class ExitCommand : Command
    {
        private readonly IApplication _application;

        public ExitCommand(IApplication application)
        {
            _application = application;
            Text = Resources.Exit;
        }

        public override void Run()
        {
            _application.Shutdown();
        }
    }
}
