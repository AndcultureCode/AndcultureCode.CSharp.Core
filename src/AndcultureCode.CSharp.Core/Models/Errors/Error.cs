using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces;

namespace AndcultureCode.CSharp.Core.Models
{
    public class Error : IError
    {
        /// <summary>
        /// Gets or sets the error type/
        /// </summary>
        public ErrorType ErrorType { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public string    Key       { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string    Message   { get; set; }
    }
}