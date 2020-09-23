using System.Collections.Generic;
using System.Linq;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;

namespace AndcultureCode.CSharp.Core.Models
{
    public class Result<T> : IResult<T>
    {
        #region Properties

        public virtual int                        ErrorCount     => Errors != null ? Errors.Count : 0;
       
        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        public virtual List<IError>               Errors         { get; set; }
        public virtual bool                       HasErrors      => Errors != null && Errors.Any();
        public virtual Dictionary<string, string> NextLinkParams { get; set; }
        
        /// <summary>
        /// Gets or sets the result object.
        /// </summary>
        public virtual T                          ResultObject   { get; set; }

        #endregion Properties


        #region Constructors

        public Result() {}
        public Result(string errorMessage)                  => this.AddError(errorMessage);
        public Result(string errorKey, string errorMessage) => this.AddError(errorKey, errorMessage);
        public Result(T resultObject)                       => this.ResultObject = resultObject;

        #endregion Constructors
    }
}
