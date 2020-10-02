using System;
using AndcultureCode.CSharp.Core.Interfaces.Hosting;
using AndcultureCode.CSharp.Core.Utilities.Configuration;

namespace AndcultureCode.CSharp.Core.Extensions
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
    /// </summary>
    public static class IAndcultureCodeWebHostBuilderExtensions
    {
        #region Constants

        /// <summary>
        /// Literal string representation of the Core Environment
        /// </summary>
        public const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

        #endregion Constants


        #region Public Methods

        /// <summary>
        /// Configures resources from Amazon Elastic Beanstalk (EB) before AspNetCore
        /// is started up. Namely, reading of ASPNET environment
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="stdoutEnabled">Should errors be output to standard output for debugging being this could be run before logging starts</param>
        /// <param name="configurationProvider"> TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
        /// </param>
        /// <returns></returns>
        public static IAndcultureCodeWebHostBuilder PreloadAmazonElasticBeanstalk(
            this IAndcultureCodeWebHostBuilder builder,
            bool stdoutEnabled = false,
            AmazonEBConfigurationProvider configurationProvider = null)
        {
            var ebProvider = configurationProvider ?? new AmazonEBConfigurationProvider(stdoutEnabled);
            if (ebProvider.Has(ASPNETCORE_ENVIRONMENT))
            {
                var ebEnvironment = ebProvider.Get(ASPNETCORE_ENVIRONMENT);
                Environment.SetEnvironmentVariable(ASPNETCORE_ENVIRONMENT, ebEnvironment);
            }

            return builder;
        }

        #endregion Public Methods
    }
}