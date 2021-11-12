// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;

using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Presentation.CommandModel;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.Exit)]
    internal class ExitCommand : Command
    {
        private readonly IApplication _application;

        [ImportingConstructor]
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
