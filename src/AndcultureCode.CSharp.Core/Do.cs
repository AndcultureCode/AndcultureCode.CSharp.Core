using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace AndcultureCode.CSharp.Core
{
    /// <summary>
    /// TODO: Backfill tests https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/15
    /// Helper class to implement the Try/Catch pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Do<T>
    {
        #region Properties

        /// <summary>
        /// The thrown exception (if it exists)
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// The result of the <see cref="Try(ILogger,Func{IResult{T},T})"/> method
        /// </summary>
        public IResult<T> Result { get; private set; }

        /// <summary>
        /// Function to perform
        /// </summary>
        public Func<IResult<T>, T> Workload { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Instantiates a <see cref="Do{T}"/> object to perform a given workload
        /// </summary>
        /// <param name="workload"></param>
        public Do(Func<IResult<T>, T> workload)
        {
            Result = new Result<T>();
            Workload = workload;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// If an exception is present and it is of the TException type, than the handler will be called
        /// </summary>
        /// <typeparam name="TException">Type of the handled exception</typeparam>
        /// <param name="handler">Action that handles the exception</param>
        /// <returns></returns>
        public Do<T> Catch<TException>(Action<TException, IResult<T>> handler)
            where TException : Exception
        {
            if (Exception == null)
            {
                return this;
            }

            if (Exception.GetType() == typeof(TException)
                || typeof(TException) == typeof(Exception))
            {
                handler((TException)Exception, Result);
            }

            return this;
        }

        /// <summary>
        /// Extension of 'Finally' that will automatically log any thrown exceptions
        /// </summary>
        /// <param name="logger">Logger to use when an unhandled exception is caught</param>
        /// <param name="workload"></param>
        /// <returns></returns>
        public Do<T> Finally(ILogger logger, Action<IResult<T>> workload)
        {
            try
            {
                workload(Result);
            }
            catch (Exception ex)
            {
                Result.AddExceptionError(ex.GetType().Name, ex);
                logger?.LogError(Result.ListErrors());
            }

            return this;
        }

        /// <summary>
        /// Execute a workload to clean up after <see cref="Try(ILogger,Func{IResult{T},T})"/>
        /// </summary>
        /// <param name="workload"></param>
        /// <returns></returns>
        public Do<T> Finally(Action<IResult<T>> workload) => Finally(logger: null, workload: workload);

        /// <summary>
        /// Extension of 'Try' that will automatically log any thrown exceptions
        /// </summary>
        /// <param name="logger">Logger to use when an unhandled exception is caught</param>
        /// <param name="workload"></param>
        /// <returns></returns>
        public static Do<T> Try(ILogger logger, Func<IResult<T>, T> workload)
        {
            var d = new Do<T>(workload);

            try
            {
                d.Result.ResultObject = workload(d.Result);
            }
            catch (Exception ex)
            {
                d.Exception = ex;

                // Add the exception to the IResult object by default
                d.Result.AddExceptionError(ex.GetType().Name, ex);
                logger?.LogError(d.Result.ListErrors());
            }

            return d;
        }

        /// <summary>
        /// Tries to run the given workload and returns the result or an exception
        /// </summary>
        /// <param name="workload"></param>
        /// <returns></returns>
        public static Do<T> Try(Func<IResult<T>, T> workload) => Try(logger: null, workload: workload);

        /// <summary>
        /// Tries to run the given workload the indicated number of times
        /// </summary>
        /// <param name="logger">Logger used to log errors with</param>
        /// <param name="workload">Workload to be performed</param>
        /// <param name="retry">Number of retries that should be performed. If value is
        ///                     zero, will not retry</param>
        /// <returns></returns>
        public static Do<T> Try(ILogger logger, uint retry, Func<IResult<T>, T> workload)
        {
            if (retry == 0)
            {
                return Try(logger, workload);
            }

            var attempts = 0;
            Do<T> d = null;

            while (attempts != retry)
            {
                d = Try(logger, workload);

                if (!d.Result.HasErrors)
                {
                    break;
                }

                attempts++;
            }

            return d;
        }

        /// <summary>
        /// Extension of 'Try' that will automatically log before, during and after seeding logic
        /// </summary>
        /// <param name="seeds">Logger to use when an unhandled exception is caught</param>
        /// <param name="workload"></param>
        /// <param name="seedName">Manually supply name of seed. By default, the invoking function name is used.</param>
        /// <returns></returns>
        public static Do<T> TrySeed<TContext>(
            SeedsBase<TContext> seeds,
            Func<IResult<T>, T> workload,
            string seedName = null
        )
            where TContext : class, IContext
        {
            var d = new Do<T>(workload);

            // Default seedName to invoking method name
            if (string.IsNullOrWhiteSpace(seedName))
            {
                seedName = new StackTrace().GetFrame(1).GetMethod().Name;
            }

            try
            {
                seeds.LogStart(seedName);
                d.Result.ResultObject = workload(d.Result);
            }
            catch (Exception ex)
            {
                d.Exception = ex;

                // Add the exception to the IResult object by default
                d.Result.AddExceptionError(ex.GetType().Name, ex);
                seeds.Log(seedName, d.Result.ListErrors());
            }

            seeds.LogEnd(seedName);

            return d;
        }

        #endregion Public Methods
    }
}