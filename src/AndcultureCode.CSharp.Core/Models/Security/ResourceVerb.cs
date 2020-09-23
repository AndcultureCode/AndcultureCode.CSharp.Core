using System;

namespace AndcultureCode.CSharp.Core.Models.Security
{
    public struct ResourceVerb
    {
        #region Properties

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        public string Resource { get; set; }
        
        /// <summary>
        /// Gets or sets the verb.
        /// </summary>
        public string Verb { get; set; }

        #endregion Properties

        #region Constructor

        public ResourceVerb(string resource, string verb)
        {
            Resource = resource;
            Verb = verb;
        }

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

        public override string ToString() => $"{Resource}_{Verb}";

        #endregion Public Methods
    }
}
