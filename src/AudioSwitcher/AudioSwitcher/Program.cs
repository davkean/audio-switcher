// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using AudioSwitcher.ApplicationModel;
using System.Runtime.InteropServices;

namespace AudioSwitcher
{
    internal class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetProcessDPIAware();
        
        public static void Main(string[] parameters)
        {
            SetProcessDPIAware();

            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            using (CompositionContainer container = new CompositionContainer(catalog))
            {
                IApplication application = container.GetExportedValue<IApplication>();
                application.Run();
            }
        }
    }
}
