// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.Linq;
using System.ComponentModel.Composition;

namespace AudioSwitcher.Presentation.CommandModel
{
    [Export(typeof(CommandManager))]
    internal class CommandManager
    {
        private readonly Lazy<Command, ICommandMetadata>[] _commands;

        [ImportingConstructor]
        public CommandManager([ImportMany]Lazy<Command, ICommandMetadata>[] commands)
        {
            _commands = commands;
        }

        public Command FindCommand(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            return _commands.Where(c => c.Metadata.Id == id)
                            .Select(c => c.Value)
                            .SingleOrDefault();
        }
    }
}
