using System;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Models
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    public abstract class Lockable : Auditable, ILockable
    {
        #region ILockable Implementation

        #region Public Members

        /// <summary>
        /// The identifier of the user who locked the record
        /// </summary>
        public long? LockedById { get; set; }

        /// <summary>
        /// The date and time of when the record was locked
        /// </summary>
        public DateTimeOffset? LockedOn { get; set; }

        /// <summary>
        /// The date and time for when the record will stop being locked
        /// </summary>
        public DateTimeOffset? LockedUntil { get; set; }

        #endregion Public Members

        #region Computed Members

        /// <summary>
        /// Calculated field based on if LockedUntil (when not null) is in the past or future.
        /// </summary>
        public bool IsLocked => DetermineIfLocked();

        #endregion Computed Members

        #endregion ILockable Implementation

        #region Private Methods

        /// <summary>
        /// A record is considered "locked" if the LockedUntil field is not null
        /// and is set to a time in the future
        /// </summary>
        /// <returns>true if LockedUntil is not null and is set to a time in the future, false otherwise</returns>
        private bool DetermineIfLocked()
        {
            var currentTime = DateTimeOffset.Now;
            if (this.LockedUntil.HasValue)
            {
                return this.LockedUntil.Value > currentTime;
            }

            return false;
        }

        #endregion Private Methods
    }
}
