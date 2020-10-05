using System;

namespace AndcultureCode.CSharp.Core.Models.Security
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    public struct ResourceVerb
    {
        #region Properties

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public string Resource { get; set; }

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
        public ResourceVerb(string resource, string verb)
        {
            Resource = resource;
            Verb = verb;
        }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="resourceVerb"></param>
        /// <exception cref="ArgumentException"></exception>
        public ResourceVerb(string resourceVerb)
        {
            var lastIndex = resourceVerb.LastIndexOf("_", StringComparison.CurrentCultureIgnoreCase);

            if (lastIndex == -1)
            {
                throw new ArgumentException($"{nameof(resourceVerb)} must contain at least one underscore");
            }

            Resource = resourceVerb.Substring(0, lastIndex);
            Verb = resourceVerb.Substring(lastIndex + 1);
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Resource}_{Verb}";

        #endregion Public Methods
    }
}
