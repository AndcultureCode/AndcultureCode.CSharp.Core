using AndcultureCode.CSharp.Core.Enumerations;

namespace AndcultureCode.CSharp.Core.Interfaces
{
    public interface IError
    {
        /// <summary>
        /// Gets or sets the error type.
        /// </summary>
        ErrorType ErrorType { get; set; }
        
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        string    Key       { get; set; }
       
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        string    Message   { get; set; }
    }
}
