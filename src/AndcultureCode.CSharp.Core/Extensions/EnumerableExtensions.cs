using System.Collections.Generic;
using System.Linq;

namespace AndcultureCode.CSharp.Core.Extensions
{
    // TODO: Move to AndcultureCode.CSharp.Extensions
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Convenience method so joining strings reads better :)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> list, string delimiter = ", ")
            => list?.ToList().Join(delimiter);

        /// <summary>
        /// Convenience method for joining dictionary key values into a string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="keyValueDelimiter"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<KeyValuePair<string, string>> list, string keyValueDelimiter, string delimiter = ", ")
            => list?.Select(e => e.Join(keyValueDelimiter)).Join(delimiter);

        /// <summary>
        /// Convenience method so joining a list of strings
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this List<string> list, string delimiter = ", ")
            => list == null ? null : string.Join(delimiter, list);


        /// <summary>
        /// Convenience method so joining key value pairs
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Join(this KeyValuePair<string, string> pair, string delimiter)
            => new List<string> { pair.Key, pair.Value }
                .Where(e => !string.IsNullOrEmpty(e))
                .Join(delimiter);
    }
}
