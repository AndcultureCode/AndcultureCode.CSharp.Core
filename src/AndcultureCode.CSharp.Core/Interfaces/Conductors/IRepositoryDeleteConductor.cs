using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Conductors
{
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
        /// v
        /// </summary>
        /// <param name="items"></param>
        /// <param name="deletedById"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        IResult<bool> BulkDelete(IEnumerable<T> items, long? deletedById = default(long?), bool soft = true);

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deletedById"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        IResult<bool> Delete(long id, long? deletedById = null, bool soft = true);

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="o"></param>
        /// <param name="deletedById"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        IResult<bool> Delete(T o, long? deletedById = null, bool soft = true);

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="items"></param>
        /// <param name="deletedById"></param>
        /// <param name="batchSize"></param>
        /// <param name="soft"></param>
        /// <returns></returns>
        IResult<bool> Delete(IEnumerable<T> items, long? deletedById = null, long batchSize = 100, bool soft = true);

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        IResult<bool> Restore(T o);
        IResult<bool> Restore(long id);

        #endregion Methods
            
    }
}
