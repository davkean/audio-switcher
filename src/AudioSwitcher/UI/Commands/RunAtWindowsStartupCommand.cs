// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;
using System.IO;
using AudioSwitcher.ApplicationModel;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Win32;
using Microsoft.Win32;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.RunAtWindowsStartup)]
    internal class RunAtWindowsStartupCommand : Command
    {
        private readonly IApplication _application;

        [ImportingConstructor]
        public RunAtWindowsStartupCommand(IApplication application)
            : base(Resources.RunAtStartup)
        {
            _application = application;
        }

        public override void Refresh()
        {
            IsChecked = IsRunAtWindowsStartup(out bool localMachine);

            // We don't let the user uncheck IsChecked if its machine-wide
            if (IsChecked)
            {
                IsEnabled = !localMachine;
            }
            else
            {
                IsEnabled = true;
            }
        }

        private string RunAtWindowsStartupValue
        {
            get { return '"' + _application.ExecutablePath + "\""; }
        }

        private string RunAtWindowsStartupValueName
        {
            get { return Path.GetFileNameWithoutExtension(_application.ExecutablePath); }
        }

        public override void Run()
        {
            // Toggle the startup setting
            if (IsChecked)
            {
                DeleteRunAtWindowsStartup();
            }
            else
            {
                SetRunAtWindowsStartup();
            }
            
        }

        private bool IsRunAtWindowsStartup(out bool localMachine)
        {
            if (IsRunAtWindowsStartup(Registry.LocalMachine))
            {
                localMachine = true;
                return true;
            }

            localMachine = false;
            return IsRunAtWindowsStartup(Registry.CurrentUser);
        }

        private bool IsRunAtWindowsStartup(RegistryKey root)
        {
            using (RegistryKey key = GetRunAtWindowsStartupRegistryKey(root, writable: false))
            {
                if (key != null)
                {
                    if (key.TryGetValue(RunAtWindowsStartupValueName, out string value))
                    {
                        return string.Equals(value, RunAtWindowsStartupValue, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }

            return false;
        }

        private void DeleteRunAtWindowsStartup()
        {
            using (RegistryKey key = GetRunAtWindowsStartupRegistryKey(writable: true))
            {
                if (key != null)
                {
                    key.TryDeleteValue(RunAtWindowsStartupValueName);
                }
            }
        }

        private void SetRunAtWindowsStartup()
        {
            using (RegistryKey key = GetRunAtWindowsStartupRegistryKey(writable: true))
            {
                if (key != null)
                {
                    key.TrySetValue(RunAtWindowsStartupValueName, RunAtWindowsStartupValue);
                }
            }
        }

        private RegistryKey GetRunAtWindowsStartupRegistryKey(bool writable)
        {
            return GetRunAtWindowsStartupRegistryKey(Registry.CurrentUser, writable);
        }

        private RegistryKey GetRunAtWindowsStartupRegistryKey(RegistryKey root, bool writable)
        {
            root.TryOpenSubkey(@"Software\Microsoft\Windows\CurrentVersion\Run", writable, out RegistryKey key);

            return key;
        }
    }
}
