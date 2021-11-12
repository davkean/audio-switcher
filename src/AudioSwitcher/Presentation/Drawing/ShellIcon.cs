// -----------------------------------------------------------------------
// Copyright (c) David Kean. All rights reserved.
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace AudioSwitcher.Presentation.Drawing
{
    internal class ShellIcon
    {
        public static bool TryExtractIconByIdOrIndex(string fileNameAndIdOrIndex, Size size, out Icon icon)
        {
            icon = null;
            try
            {
                icon = ExtractIconByIdOrIndex(fileNameAndIdOrIndex, size);
            }
            catch (ArgumentException)
            {
            }
            catch (FormatException)
            {
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            
            return icon != null;
        }

        public static Icon ExtractIconByIdOrIndex(string fileNameAndIdOrIndex, Size size)
        {
            Icon icon = ExtractIconByIdOrIndex(fileNameAndIdOrIndex);
            if (icon == null)
                return null;

            return new Icon(icon, size);
        }

        public static Icon ExtractIconByIdOrIndex(string fileNameAndIdOrIndex)
        {
            string[] parts = fileNameAndIdOrIndex.Split(new char[] { ',' }, 2);

            parts[0] = Environment.ExpandEnvironmentVariables(parts[0]);
            if (parts.Length == 1)
                return ExtractIcon(parts[0]);

            if (!int.TryParse(parts[1], out int index))
                throw new FormatException();

            if (index >= 0)
                return IconExtractor.ExtractIconByIndex(parts[0], index);

            return IconExtractor.ExtractIconById(parts[0], Math.Abs(index));
        }

        private static Icon ExtractIcon(string fileName)
        {
            // Are we looking at an icon itself?
            if (TryExtractIconFromIco(fileName, out Icon icon))
                return icon;

            // Otherwise, fall back to the first icon in the file
            return IconExtractor.ExtractIconByIndex(fileName, 0);
        }

        private static bool TryExtractIconFromIco(string fileName, out Icon icon)
        {
            try
            {
                icon = new Icon(fileName);
                return true;
            }
            catch (ArgumentException)
            {
            }
            catch (Win32Exception)
            {
            }
            
            icon = null;
            return false;
        }
    }
}
