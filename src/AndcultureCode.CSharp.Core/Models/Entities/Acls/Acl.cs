using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces.Security;

namespace AndcultureCode.CSharp.Core.Models.Entities.Acls
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    public class Acl : Auditable, IAcl
    {
        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public Permission Permission { get; set; }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public string Verb { get; set; }
    }
}