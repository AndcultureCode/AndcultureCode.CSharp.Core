using AndcultureCode.CSharp.Core.Interfaces.Hosting;
using AndcultureCode.CSharp.Core.Models.Hosting;
using Microsoft.AspNetCore;

namespace AndcultureCode.CSharp.Core.Utilities.Hosting
{
    /// <summary>
    /// Static class related to our custom <see cref="WebHost"/> builder pattern
    /// </summary>
    public static class AndcultureCodeWebHost
    {
        /// <summary>
        /// Entry point to our custom WebHost builder pattern.
        /// From here extensions methods can but hung off of IAndcultureCodeWebHostBuilder
        /// </summary>
        /// <returns></returns>
        public static IAndcultureCodeWebHostBuilder Preload(string[] args) => new AndcultureCodeWebHostBuilder(args);
    }
}
