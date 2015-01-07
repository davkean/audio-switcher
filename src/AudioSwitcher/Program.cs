// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.Runtime.InteropServices;
using AudioSwitcher.ApplicationModel;
using RGiesecke.DllExport;

namespace AudioSwitcher
{
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());

            using (CompositionContainer container = new CompositionContainer(catalog))
            {
                IApplication application = container.GetExportedValue<IApplication>();
                application.Start();
            }
        }

        [DllExport]
        public static void RunInTaskBar(
            [In] IntPtr hwnd,
            [In] IntPtr ModuleHandle,
            [In, MarshalAs(UnmanagedType.LPWStr)] string CmdLineBuffer,
            int nCmdShow)
        {
            Main();
        }
    }
}