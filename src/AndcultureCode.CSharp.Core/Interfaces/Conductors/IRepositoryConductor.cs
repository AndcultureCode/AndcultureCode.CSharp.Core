using System.Collections.Generic;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
    /// <summary>
    /// Interface of a full repository conductor for an entity of type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepositoryConductor<T> : IConductor,
        IRepositoryCreateConductor<T>,
        IRepositoryDeleteConductor<T>,
        IRepositoryReadConductor<T>,
        IRepositoryUpdateConductor<T>
    where T : AndcultureCode.CSharp.Core.Models.Entities.Entity
    {
        #region Properties

        /// <summary>
        /// Ability to set and get the underlying DbContext's command timeout
        /// </summary>
        new int? CommandTimeout { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Automatically determines if the supplied entity needs created or updated.
        /// </summary>
        IResult<IEnumerable<T>> BulkCreateOrUpdate(IEnumerable<T> item, long? createdOrUpdatedById = null);

        /// <summary>
        /// Automatically determines if the supplied entity needs created or updated.
        /// </summary>
        IResult<T> CreateOrUpdate(T item, long? createdOrUpdatedById = null);

        /// <summary>
        /// Automatically determines if the supplied entities need created or updated.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="createdOrUpdatedById"></param>
        /// <returns></returns>
        IResult<IEnumerable<T>> CreateOrUpdate(IEnumerable<T> items, long? createdOrUpdatedById = default(long?));

        #endregion Methods

    }
}
