using System;
using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models.Entities.Jobs;
using AndcultureCode.GB.Business.Core.Interfaces.Workers;

namespace AndcultureCode.CSharp.Code.Interfaces.Conductors.Jobs
{
    /// <summary>
    /// Enqueue jobs with background job library support.
    ///
    /// Using this conductor will track the jobs status and other debugging information.
    /// </summary>
    public interface IJobEnqueueConductor
    {
        #region Methods

        /// <summary>
        /// Enqueue a background job by creating a job records and
        /// enqueuing with the background job provider.
        /// </summary>
        /// <param name="workerArgs">List<object> of params needed by Worker action.</param>
        /// <param name="startedById">User Id for user initiating the job.</param>
        /// <typeparam name="TWorker">Interface for Worker to be enqueued.</typeparam>
        /// <returns>Returns true if the job can be enqueued and false if not.</returns>
        IResult<Job> Enqueue<TWorker>(List<object> workerArgs, long? startedById) where TWorker : IWorker;

        /// <summary>
        /// Enqueue a background job by creating a job records and
        /// enqueuing with the background job provider.
        /// </summary>
        /// <param name="workerType">Type object for worker implementing IWorker.
        /// The interface of the class can type can be provided.</param>
        /// <param name="workerArgs">List<object> of params needed by Worker action.</param>
        /// <param name="startedById">User Id for user initiating the job.</param>
        /// <returns></returns>
        IResult<Job> Enqueue(Type workerType, List<object> workerArgs, long? startedById);

        /// <summary>
        /// Validates that a TWorker type/interface only has one implementing class.
        /// </summary>
        /// <typeparam name="TWorker">Interface for Worker to be enqueued.</typeparam>
        /// <returns>Returns true for a valid TWorker.</returns>
        bool IsValidWorkerTypeWithImplementation<TWorker>() where TWorker : IWorker;

        #endregion
    }
}
