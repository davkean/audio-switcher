// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using AudioSwitcher.IO;
using AudioSwitcher.Presentation.Drawing.Interop;
using AudioSwitcher.Win32.InteropServices;

namespace AudioSwitcher.Presentation.Drawing
{
    /// <summary>
    /// Get icon resources (RT_GROUP_ICON and RT_ICON) from an executable module (either a .dll or an .exe file).
    /// </summary>
    internal class IconExtractor : IDisposable
    {
        private readonly ReadOnlyCollection<ResourceName> _iconNames;
        private readonly SafeModuleHandle _moduleHandle;

        public IconExtractor(SafeModuleHandle moduleHandle, IList<ResourceName> iconNames)
        {
            _moduleHandle = moduleHandle;
            _iconNames = new ReadOnlyCollection<ResourceName>(iconNames);
        }

        public SafeModuleHandle ModuleHandle
        {
            get { return _moduleHandle; }
        }
        
        /// <summary>
        /// Gets a list of icons resource names RT_GROUP_ICON;
        /// </summary>
        public ReadOnlyCollection<ResourceName> IconNames
        {
            get { return _iconNames; }
        }

        public Icon GetIconByIndex(int index)
        {
            if (index < 0 || index >= IconNames.Count)
            {
                if (IconNames.Count > 0)
                    throw new ArgumentOutOfRangeException("index", index, "Index should be in the range (0-" + IconNames.Count.ToString() + ").");
                else
                    throw new ArgumentOutOfRangeException("index", index, "No icons in the list.");
            }

            return GetIconFromLib(index);
        }

        public static IconExtractor Open(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (fileName.Length == 0)
                throw new ArgumentException(null, "fileName");

            fileName = Path.GetFullPath(fileName);
            fileName = Environment.ExpandEnvironmentVariables(fileName);

            SafeModuleHandle moduleHandle = DllImports.LoadLibraryEx(fileName, IntPtr.Zero, LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE);
            if (moduleHandle.IsInvalid)
                throw Win32Marshal.GetExceptionForLastWin32Error(fileName);

            var iconNames = new List<ResourceName>();
            DllImports.EnumResourceNames(moduleHandle, ResourceTypes.RT_GROUP_ICON, (hModule, lpszType, lpszName, lParam) =>
                {
                    if (lpszType == ResourceTypes.RT_GROUP_ICON)
                        iconNames.Add(new ResourceName(lpszName));

                    return true;
                },
                IntPtr.Zero);


            return new IconExtractor(moduleHandle, iconNames);
        }

        public static Icon ExtractIconByIndex(string fileName, int index)
        {
            using (var extractor = IconExtractor.Open(fileName))
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
            
                if (index >= extractor.IconNames.Count)
                    return null;

                return extractor.GetIconByIndex(index);
            }
        }

        public static Icon ExtractIconById(string fileName, int id)
        {
            using (var extractor = IconExtractor.Open(fileName))
            {
                if (id < 0)
                    throw new ArgumentOutOfRangeException("index");

                int count = extractor.IconNames.Count;
                for (int i = 0; i < count; i++)
                {
                    ResourceName name = extractor.IconNames[i];
                    if (name.Id != null && name.Id == id)
                    {
                        return extractor.GetIconByIndex(i);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a System.Drawing.Icon that represents RT_GROUP_ICON at the givin index from the executable module.
        /// </summary>
        /// <param name="index">The index of the RT_GROUP_ICON in the executable module.</param>
        /// <returns>Returns System.Drawing.Icon.</returns>
        private Icon GetIconFromLib(int index)
        {
            byte[] resourceData = GetResourceData(ModuleHandle, IconNames[index], ResourceTypes.RT_GROUP_ICON);
            //Convert the resouce into an .ico file image.
            using (var inputStream = new MemoryStream(resourceData))
            using (var destStream = new MemoryStream())
            {
                //Read the GroupIconDir header.
                GroupIconDir grpDir = inputStream.Read<GroupIconDir>();

                int numEntries = grpDir.Count;
                int iconImageOffset = IconInfo.SizeOfIconDir + numEntries * IconInfo.SizeOfIconDirEntry;

                destStream.Write<IconDir>(grpDir.ToIconDir());
                for (int i = 0; i < numEntries; i++)
                {
                    //Read the GroupIconDirEntry.
                    GroupIconDirEntry grpEntry = inputStream.Read<GroupIconDirEntry>();

                    //Write the IconDirEntry.
                    destStream.Seek(IconInfo.SizeOfIconDir + i * IconInfo.SizeOfIconDirEntry, SeekOrigin.Begin);
                    destStream.Write<IconDirEntry>(grpEntry.ToIconDirEntry(iconImageOffset));

                    //Get the icon image raw data and write it to the stream.
                    byte[] imgBuf = GetResourceData(ModuleHandle, grpEntry.ID, ResourceTypes.RT_ICON);
                    destStream.Seek(iconImageOffset, SeekOrigin.Begin);
                    destStream.Write(imgBuf, 0, imgBuf.Length);
                    
                    //Append the iconImageOffset.
                    iconImageOffset += imgBuf.Length;
                }
                destStream.Seek(0, SeekOrigin.Begin);
                return new Icon(destStream);
            }
        }
        /// <summary>
        /// Extracts the raw data of the resource from the module.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="resourceType">The type of the resource.</param>
        /// <returns>The resource raw data.</returns>
        private static byte[] GetResourceData(SafeModuleHandle hModule, ResourceName resourceName, ResourceTypes resourceType)
        {
            //Find the resource in the module.
            IntPtr hResInfo = IntPtr.Zero;
            try { hResInfo = DllImports.FindResource(hModule, resourceName.Value, resourceType); }
            finally { resourceName.Dispose(); }
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Load the resource.
            IntPtr hResData = DllImports.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Lock the resource to read data.
            IntPtr hGlobal = DllImports.LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Get the resource size.
            int resSize = DllImports.SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }
            //Allocate the requested size.
            byte[] buf = new byte[resSize];
            //Copy the resource data into our buffer.
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }
        /// <summary>
        /// Extracts the raw data of the resource from the module.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="resourceId">The identifier of the resource.</param>
        /// <param name="resourceType">The type of the resource.</param>
        /// <returns>The resource raw data.</returns>
        private static byte[] GetResourceData(SafeModuleHandle hModule, int resourceId, ResourceTypes resourceType)
        {
            //Find the resource in the module.
            IntPtr hResInfo = DllImports.FindResource(hModule, (IntPtr) resourceId, resourceType); 
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Load the resource.
            IntPtr hResData = DllImports.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Lock the resource to read data.
            IntPtr hGlobal = DllImports.LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Get the resource size.
            int resSize = DllImports.SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }
            //Allocate the requested size.
            byte[] buf = new byte[resSize];
            //Copy the resource data into our buffer.
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _moduleHandle.Dispose();
            }
        }
    }
}
