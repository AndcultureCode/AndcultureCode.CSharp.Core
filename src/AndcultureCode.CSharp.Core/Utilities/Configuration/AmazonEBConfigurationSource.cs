using Microsoft.Extensions.Configuration;

namespace AndcultureCode.CSharp.Core.Utilities.Configuration
{
    /// <summary>
    /// Class used to create a <see cref="IConfigurationProvider"/> that reads Amazon Elastic Beanstalk instance environment variables
    /// </summary>
    public class AmazonEBConfigurationSource : IConfigurationSource
    {
        /// <summary>
        /// Builds a <see cref="IConfigurationProvider"/> that reads Amazon Elastic Beanstalk instance environment variables
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>A new <see cref="AmazonEBConfigurationProvider"/> with default values</returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder) => new AmazonEBConfigurationProvider();
    }
}