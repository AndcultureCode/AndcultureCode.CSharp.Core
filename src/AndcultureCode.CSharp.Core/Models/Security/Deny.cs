using AndcultureCode.CSharp.Core.Enumerations;

namespace AndcultureCode.CSharp.Core.Models.Security
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    public class Deny : AccessRule
    {
        #region Properties

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public override Permission Permission => Permission.Deny;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="verb"></param>
        /// <param name="subject"></param>
        public Deny(string resource, string verb, string subject) : base(resource, verb, subject) { }

        #endregion Constructor
    }
}
