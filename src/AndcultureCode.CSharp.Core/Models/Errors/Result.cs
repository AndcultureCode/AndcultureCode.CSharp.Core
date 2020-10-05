using System.Collections.Generic;
using System.Linq;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;

namespace AndcultureCode.CSharp.Core.Models.Errors
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : IResult<T>
    {
        #region Properties

        /// <summary>
        /// Gets the number of errors, if there are any; otherwise, returns 0.
        /// </summary>
        public virtual int                        ErrorCount     => Errors != null ? Errors.Count : 0;

        /// <summary>
        /// List of errors around a request
        /// </summary>
        public virtual List<IError>               Errors         { get; set; }

        /// <summary>
        /// Returns whether or not this result has any error
        /// </summary>
        public virtual bool                       HasErrors      => Errors != null && Errors.Any();

        /// <summary>
        /// List of key value pairs to be used request the very next related Result
        /// </summary>
        public virtual Dictionary<string, string> NextLinkParams { get; set; }

        /// <summary>
        /// Actual resulting value from the request
        /// </summary>
        public virtual T                          ResultObject   { get; set; }

        #endregion Properties


        #region Constructors

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public Result() {}

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="errorMessage"></param>
        public Result(string errorMessage)                  => this.AddError(errorMessage);

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="errorKey"></param>
        /// <param name="errorMessage"></param>
        public Result(string errorKey, string errorMessage) => this.AddError(errorKey, errorMessage);

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="resultObject"></param>
        public Result(T resultObject)                       => this.ResultObject = resultObject;

        #endregion Constructors
    }
}
