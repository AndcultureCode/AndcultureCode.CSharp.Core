using System;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
    /// <summary>
    /// Interface to handle locking and unlocking <see cref="ILockable"/> records
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILockingConductor<T>
       where T : class, ILockable, IEntity
    {
        /// <summary>
        /// Updates the LockedUntil field for the record, allowing the user that locked
        /// it to have the lock for a longer amount of time.
        /// </summary>
        /// <param name="id">The record id</param>
        /// <param name="lockUntil">The time the record should be locked until</param>
        /// <param name="updatedById">The id of the user updating the record's lock time</param>
        /// <returns>the updated record if the lock is successfully extended, null otherwise</returns>
        IResult<T> ExtendLock(long id, DateTimeOffset lockUntil, long? updatedById = null);

        /// <summary>
        /// Updates the locking fields to be set, which denotes that the user locking the record should
        /// have exclusive access to modifying it for the lock's duration. This, however, does
        /// not prohibit the record from being modified by others. When a record is locked, the
        /// ValidateLock method below should be used to determine if the record can be modified. This will
        /// ensure that only the user that locked the record is actually able to modify its contents.
        /// </summary>
        /// <param name="id">The record id</param>
        /// <param name="lockUntil">The time the record should be locked until</param>
        /// <param name="lockedById">The id of the user locking the record</param>
        /// <returns>the updated record if it is successfully locked, null otherwise</returns>
        IResult<T> Lock(long id, DateTimeOffset lockUntil, long? lockedById = null);

        /// <summary>
        /// Updates the locking fields back to null, meaning any user will be able to acquire a lock
        /// so they have exclusive access to editing it.
        /// </summary>
        /// <param name="id">The record id</param>
        /// <param name="unlockedById">The id of the user unlocking the record</param>
        /// <returns>the updated record if it is successfully unlocked, null otherwise</returns>
        IResult<T> Unlock(long id, long? unlockedById = null);

        /// <summary>
        /// Used to determine whether or not the user attempting to update the record is the
        /// user that has the lock, AND that the lock is still valid (i.e. not expired).
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="currentUserId">The current user id</param>
        /// <returns>true if the lock is still active and is locked by the current user, false otherwise</returns>
        IResult<bool> ValidateLock(T item, long? currentUserId = null);
    }
}