using AndcultureCode.CSharp.Core.Constants;
using AndcultureCode.CSharp.Core.Models.Localization;

namespace AndcultureCode.CSharp.Core.Cultures
{
    /// <summary>
    /// Localization Culture for United States English (en-US)
    /// </summary>
    public class EnglishUnitedStates : Culture
    {
        /// <inheritdoc/>
        public override string Code { get => Rfc4646LanguageCodes.EN_US; }
        /// <inheritdoc/>
        public override bool IsDefault => true;
    }
}
