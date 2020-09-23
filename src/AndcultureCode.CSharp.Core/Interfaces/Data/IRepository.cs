using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Data
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
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
        /// Perform a DbContext.BulkInsert on an enumeration of T within a single transaction
        /// </summary>
        /// <param name="items"></param>
        /// <param name="createdById"></param>
        /// <returns></returns>
        IResult<List<T>> BulkCreate(IEnumerable<T> items, long? createdById = null);

        /// <summary>
        /// Calls BulkCreate() with a de-duped list of objects as determined by the
        /// property (or properties) of the object for the 'property' argument
        /// NOTE: Bulking is generally faster than batching, but locks the table.
        /// </summary>
        /// <param name="items">List of items to attempt to create</param>
        /// <param name="property">Property or properties of the typed object to determine distinctness</param>
        /// <param name="createdById">Id of the user creating the entity</param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>

        /// <summary>
        /// Calls BulkCreate() after selecting a subset of 'items' based on the distinct value of 'property'
        /// </summary>
        /// <param name="items"></param>
        /// <param name="property"></param>
        /// <param name="createdById"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        IResult<List<T>> BulkCreateDistinct<TKey>(IEnumerable<T> items, Func<T, TKey> property, long? createdById = null);

        /// <summary>
        /// Calls BulkDelete() with a de-duped list of objects as determined by the
        /// property (or properties) of the object for the 'property' argument
        /// NOTE: Bulking is generally faster than batching, but locks the table.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="deletedById"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        IResult<bool> BulkDelete(IEnumerable<T> items, long? deletedById = null, bool soft = true);
        IResult<bool> BulkUpdate(IEnumerable<T> entities, long? updatedBy = default(long?));
        IResult<T> Create(T item, long? createdById = null);
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
        IResult<bool> Delete(long id, long? deletedById = null, bool soft = true);
        IResult<bool> Delete(T o, long? deletedById = null, bool soft = true);
        IResult<bool> Delete(IEnumerable<T> items, long? deletedById = null, long batchSize = 100, bool soft = true);

        /// <summary>
        /// Find all filtered, sorted and paged
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        IResult<IQueryable<T>> FindAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool? ignoreQueryFilters = false, bool asNoTracking = false);

        /// <summary>
        /// Find all filtered, sorted and paged and converts to an IList&lt;T&gt;
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        IResult<IList<T>> FindAllCommitted(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null, bool? ignoreQueryFilters = false);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        IResult<T> FindById(long id, bool? ignoreQueryFilters = false);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IResult<T> FindById(long id, params Expression<Func<T, object>>[] includeProperties);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IResult<T> FindById(long id, bool? ignoreQueryFilters = false, params Expression<Func<T, object>>[] includeProperties);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IResult<T> FindById(long id, params string[] includeProperties);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        IResult<bool> Restore(T o);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult<bool> Restore(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        IResult<bool> Update(T item, long? updatedBy = null);

        /// <summary>
        /// Calls Update one-by-one for each item in the enumerated entities.
        /// For large operations, BulkUpdate() is more efficient.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="updatedBy"></param>
        /// <returns>True if entities updated without any exceptions. False if an exception was thrown.</returns>
        IResult<bool> Update(IEnumerable<T> entities, long? updatedBy = default(long?));

        #endregion Methods
    }
}