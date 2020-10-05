using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using AndcultureCode.CSharp.Core.Interfaces.Hosting;

namespace AndcultureCode.CSharp.Core.Models.Hosting
{
    /// <summary>
    /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
    /// </summary>
    public class AndcultureCodeWebHostBuilder : IAndcultureCodeWebHostBuilder
    {
        #region Properties

        /// <summary>
        /// The command line args to dotnet process. Ultimately piped to AspNetCore WebHost.
        /// </summary>
        public string[] Args { get; set; }

        #endregion Properties


        #region Constructors

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        public AndcultureCodeWebHostBuilder() {}

        /// <summary>
        /// TODO https://github.com/AndcultureCode/AndcultureCode.CSharp.Core/issues/38
        /// </summary>
        /// <param name="args"></param>
        public AndcultureCodeWebHostBuilder(string[] args)
        {
            Args = args;
        }

        #endregion Constructors


        #region Public Methods

        /// <summary>
        /// Simple wrapper around AspNetCore WebHost.CreateDefaultBuilder
        /// to support our own extensibility model
        /// </summary>
        /// <returns></returns>
        public IWebHostBuilder CreateDefaultBuilder() => WebHost.CreateDefaultBuilder(Args);

        #endregion Public Methods
    }
}