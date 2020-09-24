using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Models.Localization;

namespace AndcultureCode.CSharp.Core.Interfaces
{
    /// <summary>
    /// TODO: Fill in XML comment with interface summary
    /// </summary>
    public interface ICulture
    {
        #region Properties

        /// <summary>
        /// RFC-4646 5-character Culture code (xx-XX)
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Is this the default locale in the application? There can only be one
        /// </summary>
        bool IsDefault { get; }

        #endregion Properties


        #region Navigation Properties

        /// <summary>
        /// Gets the culture translations.
        /// </summary>
        List<CultureTranslation> CultureTranslations { get; }

        #endregion Navigation Properties
    }
}
