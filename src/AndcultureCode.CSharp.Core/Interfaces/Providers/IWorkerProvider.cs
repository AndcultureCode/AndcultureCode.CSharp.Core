using AndcultureCode.CSharp.Core.Constants;
using AndcultureCode.CSharp.Core.Models.Entities.Worker;
using System;
using System.Linq.Expressions;

namespace AndcultureCode.CSharp.Core.Interfaces.Providers.Worker
{
    /// <summary>
    /// Background job processing provider
    /// </summary>
    public interface IWorkerProvider : IProvider
    {
        #region Methods

        /// <summary>
        /// Changes the state of an enqueued job to deleted. Will still be counted in the EnqueuedCount
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(string id);

        /// <summary>
        /// Number of jobs in the deleted state.
        /// </summary>
        /// <returns></returns>
        long DeletedCount();

        /// <summary>
        /// Create a new fire and forget worker task
        /// </summary>
        /// <param name="methodCall"></param>
        string Enqueue(Expression<Action> methodCall);

        /// <summary>
        /// Create a new fire and forget worker task
        /// </summary>
        /// <param name="methodCall"></param>
        string Enqueue<T>(Expression<Action<T>> methodCall);

        /// <summary>
        /// Enqueued can still mean deleted. This includes all jobs enqueued regardless of state.
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        long EnqueuedCount(string queue = Queue.DEFAULT);

        // TODO: Not using so commenting out for now to reduce dependencies.
        /// <summary>
        /// Retrieves basic details about a given job id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // Result<IJobDetails> GetJob(string id);

        // TODO: Not using so commenting out for now to reduce dependencies.
        ///// <summary>
        ///// Retrieves basic details about a given job id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        // Result<IRecurringJobDetails> GetRecurringJob(string id);

        /// <summary>
        /// Create a recurring worker task
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="methodCall"></param>
        /// <param name="recurringOptions"></param>
        void Recur(string identifier, Expression<Action> methodCall, RecurringOption recurringOptions);

        /// <summary>
        /// Create a recurring worker task
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="methodCall"></param>
        /// <param name="chronExpression"></param>
        void Recur<T>(string identifier, Expression<Action<T>> methodCall, string chronExpression);

        /// <summary>
        /// Create a recurring worker task
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="methodCall"></param>
        /// <param name="recurringOptions"></param>
        void Recur<T>(string identifier, Expression<Action<T>> methodCall, RecurringOption recurringOptions);

        /// <summary>
        /// Number of jobs that have been setup for recurrence.
        /// </summary>
        /// <returns></returns>
        long RecurringCount();

        /// <summary>
        /// Remove a worker recurrence
        /// </summary>
        /// <param name="identifier"></param>
        void RemoveRecurrence(string identifier);

        /// <summary>
        /// Schedule a method to be enqueued at a specific time
        /// </summary>
        /// <param name="methodCall"></param>
        /// <param name="delay"></param>
        string Schedule(Expression<Action> methodCall, TimeSpan delay);

        /// <summary>
        /// Schedule a method to be enqueued at a specific time
        /// </summary>
        /// <param name="methodCall"></param>
        /// <param name="delay"></param>
        string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay);

        /// <summary>
        /// Schedule a method to be enqueued at a specific time
        /// </summary>
        /// <param name="methodCall"></param>
        /// <param name="enqueueOn"></param>
        string Schedule(Expression<Action> methodCall, DateTimeOffset enqueueOn);

        /// <summary>
        /// Schedule a method to be enqueued at a specific time
        /// </summary>
        /// <param name="methodCall"></param>
        /// <param name="enqueueOn"></param>
        string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset enqueueOn);

        #endregion Methods
    }
}
