// -----------------------------------------------------------------------
// Copyright (c) David Kean.
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
            IsChecked = IsRunAtWindowsStartup();
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

        private bool IsRunAtWindowsStartup()
        {
            using (RegistryKey key = GetRunAtWindowsStartupRegistryKey(writable: false))
            {
                if (key != null)
                {
                    string value;
                    if (key.TryGetValue(RunAtWindowsStartupValueName, out value))
                    {
                        return String.Equals(value, RunAtWindowsStartupValue, StringComparison.OrdinalIgnoreCase);
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
            RegistryKey key;
            Registry.CurrentUser.TryOpenSubkey(@"Software\Microsoft\Windows\CurrentVersion\Run", writable, out key);

            return key;
        }
    }
}
