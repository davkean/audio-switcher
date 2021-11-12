// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition;
using System.Threading;

namespace AudioSwitcher.ApplicationModel
{
    [StartupService(Priority=-1)]
    internal class SingleInstanceStartupService : IStartupService, IDisposable
    {
        private readonly bool _isFirstInstance;
        private readonly Mutex _mutex;

        [ImportingConstructor]
        public SingleInstanceStartupService(IApplication application)
        {
            // Mutex names must be under 260 chars, and can't contain backslashes, apart from "Global\" and "Local\".
            string mutexName = application.ExecutablePath.Replace('\\', '_');
            _mutex = new Mutex(true, mutexName, out _isFirstInstance);
        }

        public bool Run()
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
