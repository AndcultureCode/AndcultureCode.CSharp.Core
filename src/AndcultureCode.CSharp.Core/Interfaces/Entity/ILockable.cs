using System;

namespace AndcultureCode.CSharp.Core.Interfaces.Entity
{
    public interface ILockable
    {
        /// <summary>
        /// Gets if this instance is locked.
        /// </summary>
        bool IsLocked { get; }
        
        /// <summary>
        /// Gets or sets locked by id.
        /// </summary>
        long? LockedById { get; set; }
        
        /// <summary>
        /// Gets or sets locked on.
        /// </summary>
        DateTimeOffset? LockedOn { get; set; }
        
        /// <summary>
        /// Gets or sets locked until.
        /// </summary>
        DateTimeOffset? LockedUntil { get; set; }
    }
}