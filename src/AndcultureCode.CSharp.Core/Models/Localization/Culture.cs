using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AndcultureCode.CSharp.Extensions;
using Newtonsoft.Json;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Extensions;

namespace AndcultureCode.CSharp.Core.Models.Localization
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    public abstract class Culture : ICulture
    {
        #region Private Properties

        private string[] _cultureFilePaths
        {
            get => Directory.GetFiles(_culturesDirectory, $"*.{Code}.json");
        }

        private string _culturesDirectory = Path.Combine(
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            "Cultures"
        );

        private JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        private List<CultureTranslation> _translations;

        #endregion Private Properties


        #region Public Properties

        /// <summary>
        /// RFC-4646 5-character Culture code (xx-XX)
        /// </summary>
        public abstract string Code { get; }

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <exception cref="Exception"></exception>
        public List<CultureTranslation> CultureTranslations
        {
            get
            {
                if (_translations != null)
                {
                    return _translations;
                }

                _translations = new List<CultureTranslation>();

                if (_cultureFilePaths.IsNullOrEmpty())
                {
                    return _translations;
                }

                var conflictErrors = new List<string>();

                foreach (var filePath in _cultureFilePaths)
                {
                    var newTranslations = filePath.LoadTranslations(this.Code, _serializerSettings);
                    var conflicts = GetConflicts(_translations, newTranslations);

                    conflictErrors.AddRange(conflicts);
                    _translations.AddRange(newTranslations);
                }

                if (conflictErrors.Any())
                {
                    var errorsAsString = string.Join(",\r\n", conflictErrors);
                    throw new Exception($"Duplicate translation keys:\r\n{errorsAsString}");
                }

                return _translations;
            }
            set { _translations = value; }
        }

        /// <summary>
        /// Is this the default locale in the application? There can only be one
        /// </summary>
        public virtual bool IsDefault { get => false; }

        #endregion Public Properties


        #region Private Methods

        private List<string> GetConflicts(List<CultureTranslation> currentTranslations, List<CultureTranslation> newTranslations)
        {
            var errorMessages = new List<string>();

            foreach (var newTranslation in newTranslations)
            {
                var conflictingTranslations = currentTranslations.Where(e => e.Key == newTranslation.Key).ToList();
                if (conflictingTranslations.IsEmpty())
                {
                    continue;
                }
                conflictingTranslations.Add(newTranslation);

                var conflictingFilePaths = string.Join(", ", conflictingTranslations.Select(e => e.FilePath));

                errorMessages.Add($"Key: {newTranslation.Key}, Conflicts: {conflictingFilePaths}");
            }

            return errorMessages;
        }

        #endregion Private Methods
    }
}
