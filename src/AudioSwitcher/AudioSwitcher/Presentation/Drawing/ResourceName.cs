// -----------------------------------------------------------------------
// Copyright (c) David Kean and Abdallah Gomah.
// -----------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;

namespace AudioSwitcher.Presentation.Drawing
{
    /// <summary>
    /// Represents a resource name (either integer resource or string resource).
    /// </summary>
    public class ResourceName : IDisposable
    {
        private int? _id;
        /// <summary>
        /// Gets the resource identifier, returns null if the resource is not an integer resource.
        /// </summary>
        public int? Id
        {
            get { return _id; }
            private set { _id = value; }
        }

        private string _name;
        /// <summary>
        /// Gets the resource name, returns null if the resource is not a string resource.
        /// </summary>
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        private IntPtr _value;
        /// <summary>
        /// Gets a pointer to resource name that can be used in FindResource function.
        /// </summary>
        public IntPtr Value
        {
            get
            {
                if (this.IsIntResource)
                    return new IntPtr(this.Id.Value);

                if (this._value == IntPtr.Zero)
                    this._value = Marshal.StringToHGlobalAuto(this.Name);

                return _value;
            }
            private set { _value = value; }
        }

        /// <summary>
        /// Gets whether the resource is an integer resource.
        /// </summary>
        public bool IsIntResource
        {
            get { return (this.Id != null); }
        }

        /// <summary>
        /// Initializes a new AudioSwitcher.Presentation.Drawing.ResourceName object.
        /// </summary>
        /// <param name="lpName">Specifies the resource name. For more ifnormation, see the Remarks section.</param>
        /// <remarks>
        /// If the high bit of lpszName is not set (=0), lpszName specifies the integer identifier of the givin resource.
        /// Otherwise, it is a pointer to a null terminated string.
        /// If the first character of the string is a pound sign (#), the remaining characters represent a decimal number that specifies the integer identifier of the resource. For example, the string "#258" represents the identifier 258.
        /// #define IS_INTRESOURCE(_r) ((((ULONG_PTR)(_r)) >> 16) == 0).
        /// </remarks>
        public ResourceName(IntPtr lpName)
        {
            if (((uint)lpName >> 16) == 0)  //Integer resource
            {
                this.Id = lpName.ToInt32();
                this.Name = null;
            }
            else
            {
                this.Id = null;
                this.Name = Marshal.PtrToStringAuto(lpName);
            }
        }
        /// <summary>
        /// Destructs the ResourceName object.
        /// </summary>
        ~ResourceName()
        {
            Dispose();
        }
        /// <summary>
        /// Returns a System.String that represents the current AudioSwitcher.Presentation.Drawing.ResourceName.
        /// </summary>
        /// <returns>Returns a System.String that represents the current AudioSwitcher.Presentation.Drawing.ResourceName.</returns>
        public override string ToString()
        {
            if (this.IsIntResource)
                return "#" + this.Id.ToString();

            return this.Name;
        }
        /// <summary>
        /// Releases the pointer to the resource name.
        /// </summary>
        public void Free()
        {
            if (this._value != IntPtr.Zero)
            {
                try { Marshal.FreeHGlobal(this._value); }
                catch { }
                this._value = IntPtr.Zero;
            }
        }
        
        /// <summary>
        /// Release the pointer to the resource name.
        /// </summary>
        public void Dispose()
        {
            Free();
        }
    }
}
