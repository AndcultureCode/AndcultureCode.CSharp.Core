using Microsoft.Extensions.Configuration;
using AndcultureCode.CSharp.Core.Utilities.Configuration;

namespace AndcultureCode.CSharp.Core.Extensions
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/37
    /// </summary>
    public static class IConfigurationBuilderExtensions
    {
        /// <summary>
        /// Configures dotnet IConfigurationBuilder to read Amazon Elastic Beanstalk instance environment variables
        /// </summary>
        /// <param name="configurationBuilder"></param>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// new ConfigurationBuilder()
        ///     .AddJsonFile("appsettings.json")
        ///     .AddAmazonElasticBeanstalk()
        ///     .AddEnvironmentVariables();
        /// </code>
        /// </example>
        public static IConfigurationBuilder AddAmazonElasticBeanstalk(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new AmazonEBConfigurationSource());
            return configurationBuilder;
        }
    }
}