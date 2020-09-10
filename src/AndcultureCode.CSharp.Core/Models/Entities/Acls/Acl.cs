using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces.Security;

namespace AndcultureCode.CSharp.Core.Models.Entities
{
    public class Acl : Auditable, IAcl
    {
        public Permission Permission { get; set; }
        public string Resource { get; set; }
        public string Subject { get; set; }
        public string Verb { get; set; }
    }
}
