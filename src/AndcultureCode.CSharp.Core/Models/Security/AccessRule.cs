using System;
using AndcultureCode.CSharp.Core.Enumerations;

namespace AndcultureCode.CSharp.Core.Models.Security
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    public abstract class AccessRule : IEquatable<AccessRule>
    {
        #region Properties

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public abstract Permission Permission { get; }

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

        #endregion Properties

        #region Constructor

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="verb"></param>
        /// <param name="subject"></param>
        protected AccessRule(string resource, string verb, string subject)
        {
            Resource = resource;
            Verb = verb;
            Subject = subject;
        }

        #endregion Constructor

        #region IEquatable<AccessRule> Implementation

        bool IEquatable<AccessRule>.Equals(AccessRule other)
            => other.Resource == Resource &&
                other.Verb == Verb &&
                other.Subject == Subject &&
                other.Permission == Permission;

        #endregion IEquatable<AccessRule> Implementation
    }
}
