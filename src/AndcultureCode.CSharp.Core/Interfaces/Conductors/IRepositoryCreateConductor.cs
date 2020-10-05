using System;
using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
    /// <summary>
    /// Interface of a generic repository with create operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryCreateConductor<T>
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
        /// Bulk creates a list of items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="createdById">Optional id of creator</param>
        /// <returns></returns>
        IResult<List<T>> BulkCreate(IEnumerable<T> items, long? createdById = null);

        /// <summary>
        /// Calls BulkCreate() with a de-duped list of objects as determined by the
        /// property (or properties) of the object for the 'property' argument
        /// NOTE: Bulking is generally faster than batching, but locks the table.
        /// </summary>
        /// <param name="items">List of items to attempt to create</param>
        /// <param name="property">Property or properties of the typed object to determine distinctness</param>
        /// <param name="createdById">Id of the user creating the item</param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        IResult<List<T>> BulkCreateDistinct<TKey>(IEnumerable<T> items, Func<T, TKey> property, long? createdById = null);

        /// <summary>
        /// Creates an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="createdById">Id of the user creating the item</param>
        /// <returns></returns>
        IResult<T>       Create(T item, long? createdById = null);
        /// <summary>
        /// Creates a list of items
        /// </summary>
        /// <param name="items"></param>
        /// <param name="createdById">Id of the user creating the item</param>
        /// <returns></returns>
        IResult<List<T>> Create(IEnumerable<T> items, long? createdById = null);

        /// <summary>
        /// Calls batched overload of Create() with a de-duped list of objects as determined by the
        /// property (or properties) of the object for the 'property' argument.
        /// NOTE: Batching is generally slower than bulking, but does not lock the table.
        /// </summary>
        /// <param name="items">List of items to attempt to create</param>
        /// <param name="property">Property or properties of the typed object to determine distinctness</param>
        /// <param name="createdById">Id of the user creating the entity</param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        IResult<List<T>> CreateDistinct<TKey>(IEnumerable<T> items, Func<T, TKey> property, long? createdById = null);

        #endregion Methods
    }
}
