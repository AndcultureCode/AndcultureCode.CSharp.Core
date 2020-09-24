using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
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
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="items"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        IResult<bool> BulkUpdate(IEnumerable<T> items, long? updatedBy = null);

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        IResult<bool> Update(T item, long? updatedBy = null);

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="items"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        IResult<bool> Update(IEnumerable<T> items, long? updatedBy = default(long?));

        #endregion Methods
    }
}