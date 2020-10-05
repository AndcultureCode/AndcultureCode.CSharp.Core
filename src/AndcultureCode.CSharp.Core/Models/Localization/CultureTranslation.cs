namespace AndcultureCode.CSharp.Core.Models.Localization
{
    /// <summary>
    /// Translation of a specific term/key for the related culture.
    ///
    /// Keys are in the language of the default culture. (Hopefully the language of this code base :)
    /// </summary>
    public class CultureTranslation
    {
        #region Properties

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public string CultureCode { get; set; }

        /// <summary>
        /// Translation file from which this translation was loaded
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Unique key identifying the translation
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Translation of message for this culture
        /// </summary>
        public string Value { get; set; }

        #endregion Properties
    }
}
