using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using AndcultureCode.CSharp.Testing;
using AndcultureCode.CSharp.Testing.Extensions;
using AndcultureCode.CSharp.Core.Models;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces.Hosting;
using AndcultureCode.CSharp.Core.Utilities.Configuration;
using Moq;
using AndcultureCode.CSharp.Testing.Tests;

namespace AndcultureCode.CSharp.Core.Tests.Unit.Extensions
{
    public class IAndcultureCodeWebHostBuilderExtensionsTest : CoreUnitTest
    {
        #region Setup

        public IAndcultureCodeWebHostBuilderExtensionsTest(ITestOutputHelper output) : base(output) { }

        #endregion Setup


        #region PreloadAmazonElasticBeanstalk

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PreloadAmazonElasticBeanstalk_With_Default_Arguments_Returns_Builder(bool stdoutEnabled)
        {
            // Arrange
            var mockBuilder = new Mock<IAndcultureCodeWebHostBuilder>();

            // Act
            var result = IAndcultureCodeWebHostBuilderExtensions.PreloadAmazonElasticBeanstalk(mockBuilder.Object, stdoutEnabled);

            // Assert
            result.ShouldBe(mockBuilder.Object);
        }

        [Fact]
        public void PreloadAmazonElasticBeanstalk_When_Contains_AspNetCore_Environment_Sets_Global_EnvironmentVariable()
        {
            // Arrange
            var expected = $"testValue{Random.Int()}";
            var mockBuilder = new Mock<IAndcultureCodeWebHostBuilder>();
            var mockProvider = new Mock<AmazonEBConfigurationProvider>();
            mockProvider.Setup(e => e.Has(IAndcultureCodeWebHostBuilderExtensions.ASPNETCORE_ENVIRONMENT)).Returns(true);
            mockProvider.Setup(e => e.Get(IAndcultureCodeWebHostBuilderExtensions.ASPNETCORE_ENVIRONMENT)).Returns(expected);

            // Act
            var result = IAndcultureCodeWebHostBuilderExtensions.PreloadAmazonElasticBeanstalk(
                builder: mockBuilder.Object,
                stdoutEnabled: false,
                configurationProvider: mockProvider.Object
            );

            // Assert
            result.ShouldNotBeNull();
            Environment.GetEnvironmentVariable(IAndcultureCodeWebHostBuilderExtensions.ASPNETCORE_ENVIRONMENT).ShouldBe(expected);
        }

        #endregion PreloadAmazonElasticBeanstalk
    }
}
