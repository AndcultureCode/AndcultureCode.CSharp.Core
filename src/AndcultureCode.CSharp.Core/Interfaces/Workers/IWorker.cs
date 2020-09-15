using System.Threading;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models.Entities.Jobs;

namespace AndcultureCode.GB.Business.Core.Interfaces.Workers
{
    /// <summary>
    /// Interface for defining worker classes.
    /// </summary>
    public interface IWorker
    {
        #region Properties

        /// <summary>
        /// Unique name to reference worker.
        /// </summary>
        /// <value></value>
        string Name { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Action handles the background job process.  i.e. The work that needs to be completed.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IResult<object> Action(Job job, CancellationToken cancellationToken);

        /// <summary>
        /// Execute handles updating the job entity, call Action, and tracking the Action result.
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IResult<bool> Execute(long jobId, CancellationToken cancellationToken);

        #endregion
    }
}
