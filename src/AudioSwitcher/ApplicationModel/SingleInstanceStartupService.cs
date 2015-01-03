// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AudioSwitcher.ApplicationModel
{
    [Export(typeof(IStartupService))]
    [ExportMetadata("Priority", -1)]
    internal class SingleInstanceStartupService : IStartupService, IDisposable
    {
        private readonly bool _isFirstInstance;
        private readonly Mutex _mutex;

        public SingleInstanceStartupService()
        {
            // Mutex names must be under 260 chars, and can't contain backslashes, apart from "Global\" and "Local\".
            string mutexName = Application.ExecutablePath.Replace('\\', '_');
            _mutex = new Mutex(true, mutexName, out _isFirstInstance);
        }

        public bool Startup()
        {
            return _isFirstInstance;
        }

        public void Dispose()
        {
            if (_isFirstInstance)
            {
                _mutex.ReleaseMutex();                
            }

            _mutex.Dispose();
        }
    }
}
