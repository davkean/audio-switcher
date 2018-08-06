// -----------------------------------------------------------------------
// Copyright (c) David Kean.
// -----------------------------------------------------------------------
using System;

namespace AudioSwitcher.Audio
{
    /// <summary>
    /// PROPERTYKEY is defined in wtypes.h
    /// </summary>
    internal struct PropertyKey
    {
        /// <summary>
        /// Format ID
        /// </summary>
        public Guid formatId;
        /// <summary>
        /// Property ID
        /// </summary>
        public int propertyId;
        /// <summary>
        /// <param name="formatId"></param>
        /// <param name="propertyId"></param>
        /// </summary>
        public PropertyKey(Guid formatId, int propertyId)
        {
            this.formatId = formatId;
            this.propertyId = propertyId;
        }
    }
}
