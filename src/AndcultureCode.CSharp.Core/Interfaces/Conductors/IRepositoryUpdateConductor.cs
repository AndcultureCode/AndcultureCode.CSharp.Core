using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
    /// <summary>
    /// Interface of a generic update repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryUpdateConductor<T>
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
        /// Bulk updates a list of items of type <typeparamref name="T"/>
        /// NOTE: Bulking is generally faster than batching, but locks the table.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="updatedBy">Optional Id of the updater</param>
        /// <returns></returns>
        IResult<bool> BulkUpdate(IEnumerable<T> items, long? updatedBy = null);
        /// <summary>
        /// Updates an item of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updatedBy">Optional Id of the updater</param>
        /// <returns></returns>
        IResult<bool> Update(T item, long? updatedBy = null);

        /// <summary>
        /// Updates a list of items of type <typeparamref name="T"/>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="updatedBy">Optional Id of the updater</param>
        /// <returns></returns>
        IResult<bool> Update(IEnumerable<T> items, long? updatedBy = default(long?));

        #endregion Methods
    }
}
