// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Forms;
using AudioSwitcher.Presentation.CommandModel;
using AudioSwitcher.Win32;
using Microsoft.Win32;

namespace AudioSwitcher.UI.Commands
{
    [Command(CommandId.ToggleRunAtWindowsStartup)]
    internal class ToggleRunAtWindowsStartupCommand : Command
    {
        private readonly static string RunAsWindowsStartupValue = '"' + Application.ExecutablePath + "\" -silent";
        private readonly static string RunAtWindowsStartupValueName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);

        [ImportingConstructor]
        public ToggleRunAtWindowsStartupCommand()
        {
            Text = Resources.RunAtStartup;
        }

        public override void UpdateStatus()
        {
            IsChecked = IsRunAtWindowsStartup();
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
                        return String.Equals(value, RunAsWindowsStartupValue, StringComparison.OrdinalIgnoreCase);
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
                    key.TrySetValue(RunAtWindowsStartupValueName, RunAsWindowsStartupValue);
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
