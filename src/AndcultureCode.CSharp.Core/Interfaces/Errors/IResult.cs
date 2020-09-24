using System.Collections.Generic;

namespace AndcultureCode.CSharp.Core.Interfaces
{
    /// <summary>
    /// TODO: Fill in XML comment with interface summary
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResult<T>
    {
        /// <summary>
        /// Number of errors
        /// </summary>
        /// <value></value>
        int ErrorCount { get; }

        /// <summary>
        /// List of errors around a request
        /// </summary>
        /// <value></value>
        List<IError> Errors { get; set; }

        /// <summary>
        /// Does this result contain any errors?
        /// </summary>
        /// <value></value>
        bool HasErrors { get; }

        /// <summary>
        /// List of key value pairs to be used request the very next related Result
        /// </summary>
        /// <value></value>
        Dictionary<string, string> NextLinkParams { get; set; }

        /// <summary>
        /// Actual resulting value from the request
        /// </summary>
        /// <value></value>
        T ResultObject { get; set; }
    }
}