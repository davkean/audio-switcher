// -----------------------------------------------------------------------
// Copyright (c) David Kean and Abdallah Gomah.
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using AudioSwitcher.IO;
using AudioSwitcher.Presentation.Drawing.Interop;

namespace AudioSwitcher.Presentation.Drawing
{
    /// <summary>
    /// Provides information about a givin icon.
    /// This class cannot be inherited.
    /// </summary>
    internal class IconInfo
    {
        #region ReadOnly
        public static int SizeOfIconDir = Marshal.SizeOf(typeof(IconDir));
        public static int SizeOfIconDirEntry = Marshal.SizeOf(typeof(IconDirEntry));
        public static int SizeOfGroupIconDir = Marshal.SizeOf(typeof(GroupIconDir));
        public static int SizeOfGroupIconDirEntry = Marshal.SizeOf(typeof(GroupIconDirEntry));
        #endregion

        #region Properties
        private Icon _sourceIcon;
        /// <summary>
        /// Gets the source System.Drawing.Icon.
        /// </summary>
        public Icon SourceIcon
        {
            get { return _sourceIcon; }
            private set { _sourceIcon = value; }
        }

        private string _fileName = null;
        /// <summary>
        /// Gets the icon's file name. 
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            private set { _fileName = value; }
        }

        private List<Icon> _images;
        /// <summary>
        /// Gets a list System.Drawing.Icon that presents the icon contained images.
        /// </summary>
        public List<Icon> Images
        {
            get { return _images; }
            private set { _images = value; }
        }

        /// <summary>
        /// Get whether the icon contain more than one image or not.
        /// </summary>
        public bool IsMultiIcon
        {
            get { return (this.Images.Count > 1); }
        }

        private int _bestFitIconIndex;
        /// <summary>
        /// Gets icon index that best fits to screen resolution.
        /// </summary>
        public int BestFitIconIndex
        {
            get { return _bestFitIconIndex; }
            private set { _bestFitIconIndex = value; }
        }

        private int _width;
        /// <summary>
        /// Gets icon width.
        /// </summary>
        public int Width
        {
            get { return _width; }
            private set { _width = value; }
        }

        private int _height;
        /// <summary>
        /// Gets icon height.
        /// </summary>
        public int Height
        {
            get { return _height; }
            private set { _height = value; }
        }

        private int _colorCount;
        /// <summary>
        /// Gets number of colors in icon (0 if >=8bpp).
        /// </summary>
        public int ColorCount
        {
            get { return _colorCount; }
            private set { _colorCount = value; }
        }

        private int _planes;
        /// <summary>
        /// Gets icon color planes.
        /// </summary>
        public int Planes
        {
            get { return _planes; }
            private set { _planes = value; }
        }

        private int _bitCount;
        /// <summary>
        /// Gets icon bits per pixel (0 if &lt; 8bpp).
        /// </summary>
        public int BitCount
        {
            get { return _bitCount; }
            private set { _bitCount = value; }
        }

        /// <summary>
        /// Gets icon bits per pixel.
        /// </summary>
        public int ColorDepth
        {
            get
            {
                if (this.BitCount != 0)
                    return this.BitCount;
                if (this.ColorCount == 0)
                    return 0;
                return (int)Math.Log(this.ColorCount, 2);
            }
        }
        #endregion

        #region Icon Headers Properties
        private IconDir _iconDir;
        /// <summary>
        /// Gets the AudioSwitcher.Presentation.Drawing.IconDir of the icon.
        /// </summary>
        public IconDir IconDir
        {
            get { return _iconDir; }
            private set { _iconDir = value; }
        }

        private GroupIconDir _groupIconDir;
        /// <summary>
        /// Gets the AudioSwitcher.Presentation.Drawing.GroupIconDir of the icon.
        /// </summary>
        public GroupIconDir GroupIconDir
        {
            get { return _groupIconDir; }
            private set { _groupIconDir = value; }
        }

        private List<IconDirEntry> _iconDirEntries;
        /// <summary>
        /// Gets a list of AudioSwitcher.Presentation.Drawing.IconDirEntry of the icon.
        /// </summary>
        public List<IconDirEntry> IconDirEntries
        {
            get { return _iconDirEntries; }
            private set { _iconDirEntries = value; }
        }

        private List<GroupIconDirEntry> _groupIconDirEntries;
        /// <summary>
        /// Gets a list of AudioSwitcher.Presentation.Drawing.GroupIconDirEntry of the icon.
        /// </summary>
        public List<GroupIconDirEntry> GroupIconDirEntries
        {
            get { return _groupIconDirEntries; }
            private set { _groupIconDirEntries = value; }
        }

        private List<byte[]> _rawData;
        /// <summary>
        /// Gets a list of raw data for each icon image.
        /// </summary>
        public List<byte[]> RawData
        {
            get { return _rawData; }
            private set { _rawData = value; }
        }

        private byte[] _resourceRawData;
        /// <summary>
        /// Gets the icon raw data as a resource data.
        /// </summary>
        public byte[] ResourceRawData
        {
            get { return _resourceRawData; }
            set { _resourceRawData = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Intializes a new instance of AudioSwitcher.Presentation.Drawing.IconInfo which contains the information about the givin icon.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon object to retrieve the information about.</param>
        public IconInfo(Icon icon)
        {
            this.FileName = null;
            LoadIconInfo(icon);
        }

        /// <summary>
        /// Intializes a new instance of AudioSwitcher.Presentation.Drawing.IconInfo which contains the information about the icon in the givin file.
        /// </summary>
        /// <param name="fileName">A fully qualified name of the icon file, it can contain environment variables.</param>
        public IconInfo(string fileName)
        {
            this.FileName = FileName;
            LoadIconInfo(new Icon(fileName));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex()
        {
            int iconIndex = 0;
            IntPtr resBits = Marshal.AllocHGlobal(this.ResourceRawData.Length);
            Marshal.Copy(this.ResourceRawData, 0, resBits, this.ResourceRawData.Length);
            try { iconIndex = DllImports.LookupIconIdFromDirectory(resBits, true); }
            finally { Marshal.FreeHGlobal(resBits); }

            return iconIndex;
        }
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex(Size desiredSize)
        {
            return GetBestFitIconIndex(desiredSize, false);
        }
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <param name="isMonochrome">Specifies whether to get the monochrome icon or the colored one.</param>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex(Size desiredSize, bool isMonochrome)
        {
            int iconIndex = 0;
            LookupIconIdFromDirectoryExFlags flags = LookupIconIdFromDirectoryExFlags.LR_DEFAULTCOLOR;
            if (isMonochrome)
                flags = LookupIconIdFromDirectoryExFlags.LR_MONOCHROME;
            IntPtr resBits = Marshal.AllocHGlobal(this.ResourceRawData.Length);
            Marshal.Copy(this.ResourceRawData, 0, resBits, this.ResourceRawData.Length);
            try { iconIndex = DllImports.LookupIconIdFromDirectoryEx(resBits, true, desiredSize.Width, desiredSize.Height, flags); }
            finally { Marshal.FreeHGlobal(resBits); }

            return iconIndex;
        }
        #endregion

        private void LoadIconInfo(Icon icon)
        {
            if (icon == null)
                throw new ArgumentNullException("icon");

            this.SourceIcon = icon;
            MemoryStream inputStream = new MemoryStream();
            this.SourceIcon.Save(inputStream);

            inputStream.Seek(0, SeekOrigin.Begin);
            IconDir dir = inputStream.Read<IconDir>();

            this.IconDir = dir;
            this.GroupIconDir = dir.ToGroupIconDir();

            this.Images = new List<Icon>(dir.Count);
            this.IconDirEntries = new List<IconDirEntry>(dir.Count);
            this.GroupIconDirEntries = new List<GroupIconDirEntry>(dir.Count);
            this.RawData = new List<byte[]>(dir.Count);

            IconDir newDir = dir;
            newDir.Count = 1;
            for (int i = 0; i < dir.Count; i++)
            {
                inputStream.Seek(SizeOfIconDir + i * SizeOfIconDirEntry, SeekOrigin.Begin);

                IconDirEntry entry = inputStream.Read<IconDirEntry>();

                this.IconDirEntries.Add(entry);
                this.GroupIconDirEntries.Add(entry.ToGroupIconDirEntry(i));

                byte[] content = new byte[entry.BytesInRes];
                inputStream.Seek(entry.ImageOffset, SeekOrigin.Begin);
                inputStream.Read(content, 0, content.Length);
                this.RawData.Add(content);

                IconDirEntry newEntry = entry;
                newEntry.ImageOffset = SizeOfIconDir + SizeOfIconDirEntry;

                MemoryStream outputStream = new MemoryStream();
                outputStream.Write<IconDir>(newDir);
                outputStream.Write<IconDirEntry>(newEntry);
                outputStream.Write(content, 0, content.Length);

                outputStream.Seek(0, SeekOrigin.Begin);
                Icon newIcon = new Icon(outputStream);
                outputStream.Close();

                this.Images.Add(newIcon);
                if (dir.Count == 1)
                {
                    this.BestFitIconIndex = 0;

                    this.Width = entry.Width;
                    this.Height = entry.Height;
                    this.ColorCount = entry.ColorCount;
                    this.Planes = entry.Planes;
                    this.BitCount = entry.BitCount;
                }
            }
            inputStream.Close();
            this.ResourceRawData = GetIconResourceData();

            if (dir.Count > 1)
            {
                this.BestFitIconIndex = GetBestFitIconIndex();

                this.Width = this.IconDirEntries[this.BestFitIconIndex].Width;
                this.Height = this.IconDirEntries[this.BestFitIconIndex].Height;
                this.ColorCount = this.IconDirEntries[this.BestFitIconIndex].ColorCount;
                this.Planes = this.IconDirEntries[this.BestFitIconIndex].Planes;
                this.BitCount = this.IconDirEntries[this.BestFitIconIndex].BitCount;
            }

        }

        private byte[] GetIconResourceData()
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                outputStream.Write(GroupIconDir);

                foreach (GroupIconDirEntry entry in this.GroupIconDirEntries)
                {
                    outputStream.Write(entry);
                }

                return outputStream.ToArray();
            }
        }
    }
}
