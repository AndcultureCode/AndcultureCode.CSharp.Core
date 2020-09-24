using System.Collections.Generic;
using System.Linq;
using AndcultureCode.CSharp.Core.Interfaces;

namespace AndcultureCode.CSharp.Core.Models.Errors
{
    public class PagedResult<T> : IResult<T>
    {
        #region Properties

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        public int ErrorCount => Errors != null ? Errors.Count : 0;

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public List<IError> Errors { get; set; }

        /// <summary>
        /// TODO: Fill in XML comment with method summary
        /// </summary>
        public bool HasErrors => Errors != null && Errors.Any();

        /// <summary>
        /// Gets or sets the next link parameters.
        /// </summary>
        public Dictionary<string, string> NextLinkParams { get; set; }

        /// <summary>
        /// Gets or sets the result object.
        /// </summary>
        public T ResultObject { get; set; }

        /// <summary>
        /// Gets or sets the row count.
        /// </summary>
        public long RowCount { get; set; }

        #endregion Properties


        #region Constructors

        public PagedResult(T rows, long rowCount) : this(rows, rowCount, null) { }
        public PagedResult(T rows, long rowCount, Dictionary<string, string> nextLinkParams)
        {
            NextLinkParams = nextLinkParams;
            ResultObject = rows;
            RowCount = rowCount;
        }

        #endregion Constructors
    }
}