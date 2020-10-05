using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AndcultureCode.CSharp.Core.Utilities.Configuration
{
    /// <summary>
    /// Adds support to read environment variables from an Amazon Elastic Beanstalk EC2 instance
    ///
    /// At this time AWS stores these environment variables in its own proprietary configuration
    /// that we are forced to read and pipe to the application.
    /// </summary>
    public class AmazonEBConfigurationProvider : ConfigurationProvider
    {
        #region Constants

        /// <summary>
        /// Absolute path to the AWS Elastic Beanstalk windows instance configuration file
        /// </summary>
        public const string CONFIGURATION_FILE_PATH = @"C:\Program Files\Amazon\ElasticBeanstalk\config\containerconfiguration";

        #endregion Constants


        #region Properties

        /// <summary>
        /// Path for the AWS Elastic Beanstalk configuration file
        /// </summary>
        public string ConfigurationFilePath { get; set; }

        /// <summary>
        /// Determines if logging to standard output should be enabled
        /// </summary>
        public bool StdoutEnabled { get; set; }

        #endregion Properties


        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonEBConfigurationProvider"></see> class with default values for <see cref="ConfigurationFilePath"/> and <see cref="StdoutEnabled"/>
        /// </summary>
        public AmazonEBConfigurationProvider() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonEBConfigurationProvider"></see> class with optional values for <see cref="ConfigurationFilePath"/> and <see cref="StdoutEnabled"/>
        /// </summary>
        /// <param name="stdoutEnabled">Enables logging to standard output</param>
        /// <param name="configurationFilePath">Path to configuration file, if not provided the value of <see cref="CONFIGURATION_FILE_PATH"/> will be used</param>
        public AmazonEBConfigurationProvider(bool stdoutEnabled = false, string configurationFilePath = null)
        {
            ConfigurationFilePath = string.IsNullOrWhiteSpace(configurationFilePath) ? CONFIGURATION_FILE_PATH : configurationFilePath;
            StdoutEnabled = stdoutEnabled;
        }

        #endregion Constructors


        #region Properties

        /// <summary>
        /// Must be static to cache initially loaded configuration across multiple requests
        /// </summary>
        public static IDictionary<string, string> CachedConfiguration { get; set; }

        #endregion Properties


        #region Public Methods

        /// <summary>
        /// Gets the value of an environment variable by the given <paramref name="key"/>
        /// </summary>
        /// <param name="key">The string identifying the requested variable</param>
        /// <returns>The environment variable or <c>null</c> if the key isn't present</returns>
        public virtual string Get(string key) => Has(key) ? Read()[key] : null;

        /// <summary>
        /// Checks if an environment variable by the given <paramref name="key"/> is present in the configuration
        /// </summary>
        /// <param name="key">The string identifying the requested variable</param>
        /// <returns><c>true</c> if the variable exists <c>false</c> otherwise</returns>
        public virtual bool Has(string key) => Read().ContainsKey(key);

        /// <summary>
        /// Reads the configuration from the <see cref="ConfigurationFilePath"/> and returns it as an IDictionary
        /// </summary>
        /// <remarks>
        /// <para>If the configuration file doesn't exist it returns an empty dictionary</para>
        /// <para>The return value might be cached</para>
        /// </remarks>
        /// <returns>A dictionary of key/values for all the environment variables in the configuration</returns>
        public virtual IDictionary<string, string> Read()
        {
            if (CachedConfiguration != null)
            {
                return CachedConfiguration;
            }

            var result = new Dictionary<string, string>();

            if (!File.Exists(ConfigurationFilePath))
            {
                LogIfEnabled($"File '{ConfigurationFilePath}' does not exist");
                return result;
            }

            string configJson;
            JArray env;
            try
            {
                configJson = File.ReadAllText(ConfigurationFilePath);
                var config = JObject.Parse(configJson);
                env = (JArray)config["iis"]?["env"];
            }
            catch (Exception ex)
            {
                LogIfEnabled($"Failed to read file contents '{ConfigurationFilePath}' - {ex.Message}");
                return result;
            }

            if (env == null || env.Count == 0)
            {
                LogIfEnabled("No environment variables found");
                return result;
            }

            foreach (var item in env.Select(i => (string)i))
            {
                int eqIndex = item.IndexOf('=');
                var key = item.Substring(0, eqIndex);
                var value = item.Substring(eqIndex + 1);
                key = key.Replace("__", ":"); // Need to translate so inheritance is respected (addition to fix the gist in github)
                result[key] = value;
            }

            CachedConfiguration = result;

            return result;
        }

        /// <summary>
        /// Load the configuration into the inherited 'Data' dictionary for use by ConfigurationRoot/Builder
        /// </summary>
        public override void Load()
        {
            foreach (var entry in Read())
            {
                Data[entry.Key] = entry.Value;
            }
        }

        #endregion Public Methods


        #region Private Methods

        private void LogIfEnabled(string message)
        {
            if (!StdoutEnabled)
            {
                return;
            }

            Console.WriteLine($"[AmazonEBConfigurationProvider] {message}");
        }

        #endregion Private Methods
    }
}