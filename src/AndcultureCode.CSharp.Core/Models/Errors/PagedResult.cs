using System.Collections.Generic;
using System.Linq;
using AndcultureCode.CSharp.Core.Interfaces;

namespace AndcultureCode.CSharp.Core.Models.Errors
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T> : IResult<T>
    {
        #region Properties

        /// <summary>
        /// Gets the number of errors, if there are any; otherwise, returns 0.
        /// </summary>
        public int                        ErrorCount     => Errors != null ? Errors.Count : 0;

        /// <summary>
        /// List of errors around a request
        /// </summary>
        public List<IError>               Errors         { get; set; }

        /// <summary>
        /// Returns whether or not this result has any errors
        /// </summary>
        public bool                       HasErrors      => Errors != null && Errors.Any();

        /// <summary>
        /// List of key value pairs to be used request the very next related Result
        /// </summary>
        public Dictionary<string, string> NextLinkParams { get; set; }

        /// <summary>
        /// Actual resulting value from the request
        /// </summary>
        public T                          ResultObject   { get; set; }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public long                       RowCount       { get; set; }

        #endregion Properties


        #region Constructors

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="rowCount"></param>
        public PagedResult(T rows, long rowCount) : this(rows, rowCount, null) {}

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="rowCount"></param>
        /// <param name="nextLinkParams"></param>
        public PagedResult(T rows, long rowCount, Dictionary<string, string> nextLinkParams)
        {
            NextLinkParams = nextLinkParams;
            ResultObject   = rows;
            RowCount       = rowCount;
        }

        #endregion Constructors
    }
}
