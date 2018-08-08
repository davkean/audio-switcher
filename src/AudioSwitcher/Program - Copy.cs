// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using AudioSwitcher.ApplicationModel;

namespace AudioSwitcher
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            using (var container = new CompositionContainer(catalog))
            {
                IApplication application = container.GetExportedValue<IApplication>();
                application.Start();
            }
        }
    }
}