using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces.Security;

namespace AndcultureCode.CSharp.Core.Models.Entities
{
    public class Acl : Auditable, IAcl
    {
        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Gets or serts the subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        public string Verb { get; set; }
    }
}