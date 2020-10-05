using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;

namespace AndcultureCode.CSharp.Core.Utilities.Localization
{
    /// <summary>
    /// Static class with helper functions related to localization
    /// </summary>
    public static class LocalizationUtils
    {
        #region Private Properties

        private static List<string> _assemblyExclusions = new List<string> { "Microsoft", "Serilog", "System", "Windows", "xunit" };
        private static List<ICulture> _cultures;

        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// Current cultures supported by the application
        /// </summary>
        public static List<ICulture> Cultures
        {
            get
            {
                if (_cultures != null)
                {
                    return _cultures;
                }

                _cultures = AppDomain
                    .CurrentDomain
                    .GetAssemblies()
                    .Where(x => _assemblyExclusions.All(e => !x.GetName().FullName.StartsWith(e))) // Avoid loading types for common assemblies out of our control
                    .SelectMany(x =>
                    {
                        // TODO: Provided the method gets added, update this to be a convenience method
                        // see https://github.com/AndcultureCode/AndcultureCode.CSharp.Extensions/issues/38
                        try
                        {
                            return x.GetTypes();
                        }
                        catch (ReflectionTypeLoadException e)
                        {
                            return e.Types.Where(t => t != null);
                        }
                    })
                    .Where(x => typeof(ICulture).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(e => Activator.CreateInstance(e))
                    .Cast<ICulture>()
                    .ToList();

                return _cultures;
            }
        }
        /// <summary>
        /// List of supported CultureInfo
        /// </summary>
        public static List<CultureInfo> CultureInfos { get => Cultures?.ToCultureInfos(); } 

        /// <summary>
        /// Default culture for the application
        /// </summary>
        public static ICulture DefaultCulture { get => Cultures?.Default(); }

        /// <summary>
        /// Code of the default culture for the application
        /// </summary>
        public static string DefaultCultureCode { get => DefaultCulture?.Code; }

        /// <summary>
        /// CultureInfo of the default culture fore the application
        /// </summary>
        public static CultureInfo DefaultCultureInfo { get => DefaultCulture?.ToCultureInfo(); }

        #endregion Public Properties


        #region Public Methods

        /// <summary>
        /// Retrieves the <see cref="ICulture"/> from the supported Cultures by <paramref name="cultureCode"/>
        /// </summary>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public static ICulture CultureByCode(string cultureCode)
        {
            if (cultureCode == CultureInfo.InvariantCulture.Name)
            {
                return DefaultCulture;
            }

            return Cultures.FirstOrDefault(e => e.Code.ToLower() == cultureCode.ToLower());
        }

        /// <summary>
        /// Retrieves the codes of the current cultures supported by the application
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns>A concatenated string delimited by <paramref name="delimiter"/></returns>
        public static string CultureCodes(string delimiter = ", ")
            => Cultures.ToCultureCodes(delimiter);

        /// <summary>
        /// Checks if a culture by the given <paramref name="cultureCode"/> is supported by the application
        /// </summary>
        /// <param name="cultureCode">The string identifying the requested culture</param>
        /// <returns></returns>
        public static bool CultureExists(string cultureCode) => Cultures.Exists(cultureCode);

        #endregion Public Methods
    }
}
