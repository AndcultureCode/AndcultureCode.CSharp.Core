using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces.Entity;

namespace AndcultureCode.CSharp.Core.Interfaces.Security
{
    public interface IAcl : IAuditable
    {
        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        Permission Permission { get; set; }
        
        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        string Resource { get; set; }
        
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        string Subject { get; set; }
        
        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        string Verb { get; set; }
    }
}