using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Extensions;

namespace AndcultureCode.CSharp.Core.Extensions
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
    /// </summary>
    public static class ICultureExtensions
    {
        #region Default

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="cultures"></param>
        /// <returns></returns>
        public static ICulture Default(this IEnumerable<ICulture> cultures)
        {
            var matches = cultures?.Where(e => e.IsDefault).ToList();
            var matchesCount = matches?.Count() ?? 0;

            if (matchesCount != 1)
            {
                throw new ArgumentException($"Cultures must have exactly 1 default culture, but has {matchesCount}");
            }

            return matches.First();
        }

        #endregion Default


        #region Exists

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="cultures"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public static bool Exists(this IEnumerable<ICulture> cultures, string cultureCode)
        {
            if (cultures.IsNullOrEmpty() || cultureCode.IsNullOrEmpty())
            {
                return false;
            }

            cultureCode = cultureCode.ToLower();

            return cultures.Any(e => e.Code.ToLower() == cultureCode);
        }

        #endregion Exists


        #region ToCultureNames

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="cultures"></param>
        /// <returns></returns>
        public static List<string> ToCultureCodes(this IEnumerable<ICulture> cultures)
             => cultures?.Select(e => e.Code).ToList();

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="cultures"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string ToCultureCodes(this IEnumerable<ICulture> cultures, string delimiter)
        {
            if (string.IsNullOrEmpty(delimiter))
            {
                throw new ArgumentException("Delimiter cannot be null or empty");
            }

            if (delimiter.Contains("-"))
            {
                throw new ArgumentException("Delimiter cannot contain hyphens being RFC4646 format uses hyphons");
            }

            return string.Join(delimiter, cultures?.ToCultureCodes());
        }

        #endregion ToCultureNames


        #region ToCultureInfo/s

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static CultureInfo ToCultureInfo(this ICulture culture)
            => new CultureInfo(culture?.Code);

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="cultures"></param>
        /// <returns></returns>
        public static List<CultureInfo> ToCultureInfos(this IEnumerable<ICulture> cultures)
            => cultures?.Select(x => x.ToCultureInfo()).ToList();

        #endregion ToCultureInfo/s
    }
}
