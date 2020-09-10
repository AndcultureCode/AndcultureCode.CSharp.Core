using System;
using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using Newtonsoft.Json;
using AndcultureCode.CSharp.Core.Enumerations;

namespace AndcultureCode.CSharp.Core.Models.Entities.Jobs
{
    /// <summary>
    /// Entity for tracking jobs enqueued through the JobEnqueueConductor.
    /// </summary>
    public class Job : Auditable
    {
        #region Properties

        /// <summary>
        /// JobId from Hangfire
        /// </summary>
        /// <value></value>
        public string BackgroundJobId { get; set; }

        /// <summary>
        /// Data collected durring processing the job.
        /// </summary>
        /// <value></value>
        public string DebugJson { get; set; }

        /// <summary>
        /// When the job is finished either by encountering an error or completing successfully.
        /// </summary>
        /// <value></value>
        public DateTimeOffset? EndedOn { get; set; }

        /// <summary>
        /// Errors encountered during processing.
        /// </summary>
        /// <value></value>
        public string Error { get; set; }

        /// <summary>
        /// When the job begins processing.
        /// </summary>
        /// <value></value>
        public DateTimeOffset? StartedOn { get; set; }

        /// <summary>
        /// Id of User who started the job.
        /// </summary>
        /// <value></value>
        public long? StartedById { get; set; }

        /// <summary>
        /// Status of the job.
        /// </summary>
        /// <value></value>
        public JobStatus? Status { get; set; }

        /// <summary>
        /// Name property of Worker class to be run for the job.
        /// </summary>
        /// <value></value>
        public string WorkerName { get; set; }

        /// <summary>
        /// Arguments needed by the Worker class's action method.
        /// </summary>
        /// <value></value>
        public string WorkerArgs { get; set; }

        #endregion Properties

        // TODO: These methods could be moved to extension methods.

        #region Methods

        /// <summary>
        /// Gets WorkerArgs and converts WorkerArgs from JSON.
        /// </summary>
        /// <returns>List<object></returns>
        public List<object> GetAndDeserializeWorkerArgs()
        {
            var workerArgsList = new List<object> { };
            var workerArgsMeta = new List<WorkerArgsMeta> { };

            if (string.IsNullOrEmpty(WorkerArgs))
            {
                return workerArgsList;
            }

            workerArgsMeta = JsonConvert.DeserializeObject<List<WorkerArgsMeta>>(WorkerArgs);

            foreach (WorkerArgsMeta e in workerArgsMeta)
            {
                if (e.Type == "null")
                {
                    workerArgsList.Add(null);
                    continue;
                }

                var type = Type.GetType(e.Type);
                var data = Convert.ChangeType(e.Data, type ?? typeof(object));

                workerArgsList.Add(data);
            }

            return workerArgsList;
        }

        /// <summary>
        /// Converts workerArgs to JSON and sets WorkerArgs on object.
        /// Objects must be built in 'System' objects.
        /// </summary>
        /// <param name="workerArgs">
        /// List<object> representing arguments for the background job.
        /// </param>
        public void SerializeAndSetWorkerArgs(List<object> workerArgs)
        {
            if (workerArgs == null)
            {
                WorkerArgs = null;
                return;
            }

            var workerArgsMeta = new List<WorkerArgsMeta>();

            foreach (object e in workerArgs)
            {
                var meta = new WorkerArgsMeta();

                if (e == null)
                {
                    meta.Data = e;
                    meta.Type = "null";

                    workerArgsMeta.Add(meta);

                    continue;
                }

                if (e?.GetType().Namespace != "System")
                {
                    throw new ArgumentException(
                        "WorkerArgs must be build-in C# types",
                        nameof(workerArgs)
                    );
                }

                meta.Data = e;
                meta.Type = e?.GetType().ToString();

                workerArgsMeta.Add(meta);
            }

            WorkerArgs = JsonConvert.SerializeObject(workerArgsMeta);
        }

        #endregion Methods

        #region Set State Methods

        /// <summary>
        /// Resets Job to initial state while keeping WorkerName, WorkerArgs, and StartedById.
        /// Use this when requeueing jobs.
        /// </summary>
        public void ResetToInitialState()
        {
            SetToInitialState(WorkerName, GetAndDeserializeWorkerArgs(), StartedById);
        }

        /// <summary>
        /// Set job to initial state after the job has been created, but before it's enqueued.
        /// </summary>
        /// <param name="workerName"></param>
        /// <param name="workerArgs"></param>
        /// <param name="startedById"></param>
        public void SetToInitialState(string workerName, List<object> workerArgs, long? startedById)
        {
            // Set job details.
            WorkerName = workerName;
            SerializeAndSetWorkerArgs(workerArgs);

            // Clear existing data.
            DebugJson = null;
            Error = null;

            // Set state.
            EndedOn = null;
            StartedById = startedById;
            StartedOn = null;
            Status = null;
        }

        /// <summary>
        /// To be set when the job's worker class Execute method begins processing the job.
        /// </summary>
        public void SetToInProgress()
        {
            if (!IsInitialState())
            {
                throw new Exception("The Job must be in the initial state to be set to in progress.");
            }

            // Set state.
            StartedOn = DateTimeOffset.Now;
            Status = JobStatus.InProgress;
        }

        /// <summary>
        /// To be set when the job has errored.
        /// </summary>
        /// <param name="error"></param>
        /// <param name="debugJson"></param>
        public void SetToErrored(
            string error = null,
            string debugJson = null
        )
        {
            if (!IsInitialState() && !IsInProgress())
            {
                throw new Exception("The Job must be in the initial state or in progress to be set to errored.");
            }

            // Add job information.
            DebugJson = string.IsNullOrEmpty(debugJson) ? null : debugJson;
            Error = string.IsNullOrEmpty(error) ? null : error;

            // Set state.
            EndedOn = DateTimeOffset.Now;
            Status = JobStatus.Errored;
        }

        /// <summary>
        /// To be set when the job has errored.
        /// This provides an override that takes the result object from the calling method.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="debugJson"></param>
        public void SetToErrored<T>(IResult<T> result, string debugJson = null)
        {
            SetToErrored(result.ListErrors(), debugJson);
        }

        /// <summary>
        /// To be set when the job has successfully completed.
        /// </summary>
        /// <param name="debugJson"></param>
        public void SetToCompleted(object debugJson)
        {
            var json = JsonConvert.SerializeObject(debugJson);

            if (!IsInProgress())
            {
                throw new Exception("The Job must be in progress to be set to errored.");
            }

            // Add job information.
            DebugJson = string.IsNullOrEmpty(json) ? null : json;

            // Set state.
            EndedOn = DateTimeOffset.Now;
            Status = JobStatus.Completed;
        }

        #endregion Set State Methods

        #region Check State Methods

        /// <summary>
        /// Test for job in initial state.
        /// </summary>
        /// <returns></returns>
        public bool IsInitialState()
        {
            return EndedOn == null
                && Status == null
                && StartedOn == null;
        }

        /// <summary>
        /// Test for job in progress.
        /// </summary>
        /// <returns></returns>
        public bool IsInProgress()
        {
            return EndedOn == null
                && Status == JobStatus.InProgress
                && StartedOn != null;
        }

        /// <summary>
        /// Test for errored job.
        /// </summary>
        /// <returns></returns>
        public bool IsErrored()
        {
            return EndedOn != null
                && Status == JobStatus.Errored;
        }

        /// <summary>
        /// Test for completed job.
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            return EndedOn != null
                && Status == JobStatus.Completed
                && StartedOn != null;
        }

        #endregion

        #region Private

        private struct WorkerArgsMeta
        {
            public object Data;
            public string Type;
        };

        #endregion
    }
}
