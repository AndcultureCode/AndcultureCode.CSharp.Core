using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
    /// <summary>
    /// Interface of a generic repository with delete operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryDeleteConductor<T>
        where T : class, IEntity
    {
        #region Properties

        /// <summary>
        /// Ability to set and get the underlying DbContext's command timeout
        /// </summary>
        int? CommandTimeout { get; set; }

        #endregion Properties


        #region Methods

        /// <summary>
        /// Bulk deletes a list of items
        /// NOTE: Bulking is generally faster than batching, but locks the table.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="deletedById">Optional Id of the deleter</param>
        /// <param name="soft">Optional flag to perform a soft delete</param>
        /// <returns></returns>
        IResult<bool> BulkDelete(IEnumerable<T> items, long? deletedById = default(long?), bool soft = true);

        /// <summary>
        /// Deletes an item by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deletedById">Optional Id of the deleter</param>
        /// <param name="soft">Optional flag to perform a soft delete</param>
        IResult<bool> Delete(long id, long? deletedById = null, bool soft = true);

        /// <summary>
        /// Deletes an item
        /// </summary>
        /// <param name="o"></param>
        /// <param name="deletedById">Optional Id of the deleter</param>
        /// <param name="soft">Optional flag to perform a soft delete</param>
        IResult<bool> Delete(T o, long? deletedById = null, bool soft = true);

        /// <summary>
        /// Bulk deletes a list of items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="deletedById">Optional Id of the deleter</param>
        /// <param name="batchSize"></param>
        /// <param name="soft">Optional flag to perform a soft delete</param>
        IResult<bool> Delete(IEnumerable<T> items, long? deletedById = null, long batchSize = 100, bool soft = true);

        /// <summary>
        /// Restores a soft deleted item
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        IResult<bool> Restore(T o);

        /// <summary>
        /// Restores a soft deleted item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult<bool> Restore(long id);

        #endregion Methods

    }
}
