using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
    /// <summary>
    /// Interface of a generic read repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryReadConductor<T>
        where T : class, IEntity
    {
        #region Properties

        /// <summary>
        /// Ability to set and get the underlying DbContext's command timeout
        /// </summary>
        int? CommandTimeout { get; set; }

        #endregion Properties


        #region Methods

        #region FindAll

        /// <summary>
        /// Configure lazy loaded queryable, given provided parameters, to load a list of <typeparamref name="T"/>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        IResult<IQueryable<T>> FindAll(
            Expression<Func<T , bool>> filter                 = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string             includeProperties              = null,
            int?               skip                           = null,
            int?               take                           = null,
            bool?              ignoreQueryFilters             = false,
            bool               asNoTracking                   = false
        );

        /// <summary>
        /// Alternative FindAll for retrieving records using NextLinkParams in place of traditional
        /// determinate pagination mechanisms, such as; skip and take.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="nextLinkParams"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        IResult<IQueryable<T>> FindAll(
            Dictionary<string, string> nextLinkParams,
            Expression<Func<T , bool>> filter                 = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool?              ignoreQueryFilters             = false,
            bool               asNoTracking                   = false
        );

        /// <summary>
        /// Similar to FindAll, this evaluates the parameters as given. The big difference here is that the query is executed
        /// inside the conductor and a List of <typeparamref name="T"/> is returned and NOT Queryable of <typeparamref name="T"/>.  This is primary used in cases where calculated
        /// fields need to be executed (committed) inside the conductor.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        IResult<IList<T>> FindAllCommitted(
            Expression<Func<T , bool>> filter                 = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string             includeProperties              = null,
            int?               skip                           = null,
            int?               take                           = null,
            bool?              ignoreQueryFilters             = false
        );

        /// <summary>
        /// Alternative FindAll for retrieving records using NextLinkParams in place of traditional
        /// determinate pagination mechanisms, such as; skip and take.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="nextLinkParams"></param>
        /// <param name="ignoreQueryFilters"></param>
        /// <returns></returns>
        IResult<IList<T>> FindAllCommitted(
            Dictionary<string, string> nextLinkParams,
            Expression<Func<T , bool>> filter                 = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool?              ignoreQueryFilters             = false
        );

        #endregion FindAll


        #region FindById

        /// <summary>
        /// Retrieves a record by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult<T> FindById(long id);

        /// <summary>
        /// Retrieves a record by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties">List of properties to include with the result</param>
        /// <returns></returns>
        IResult<T> FindById(long id, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Retrieves a record by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreQueryFilters">TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/33</param>
        /// <param name="includeProperties">List of properties to include with the result</param>
        /// <returns></returns>
        IResult<T> FindById(long id, bool ignoreQueryFilters, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Retrieves a record by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties">List of properties to include with the result</param>
        /// <returns></returns>
        IResult<T> FindById(long id, params string[] includeProperties);

        #endregion FindById

        #endregion Methods

    }
}
