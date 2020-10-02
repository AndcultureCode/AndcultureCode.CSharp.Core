using System.Collections.Generic;
using System.Linq;
using AndcultureCode.CSharp.Core.Models.Security;

namespace AndcultureCode.CSharp.Core.Extensions
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
    /// </summary>
    public static class ResourceVerbExtensions
    {
        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="resourceVerbString"></param>
        /// <returns></returns>
        public static ResourceVerb ToResourceVerb(this string resourceVerbString)
            => new ResourceVerb(resourceVerbString);

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </summary>
        /// <param name="resourceVerbStrings"></param>
        /// <returns></returns>
        public static List<ResourceVerb> ToResourceVerbs(this IEnumerable<string> resourceVerbStrings)
            => resourceVerbStrings?.Select(rv => rv.ToResourceVerb()).ToList();
    }
}
