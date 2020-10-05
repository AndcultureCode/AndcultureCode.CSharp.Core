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
using AndcultureCode.CSharp.Core.Utilities.Configuration;
using Moq;
using Microsoft.Extensions.Configuration;
using AndcultureCode.CSharp.Testing.Tests;

namespace AndcultureCode.CSharp.Core.Tests.Unit.Utilities.Configuration
{
    public class AndcultureEBConfigurationSourceTest : CoreUnitTest
    {
        #region Setup

        public AndcultureEBConfigurationSourceTest(ITestOutputHelper output) : base(output) { }

        #endregion Setup


        #region Build

        [Fact]
        public void Build_When_Null_Returns_Provider()
        {
            new AmazonEBConfigurationSource().Build(builder: null).ShouldNotBeNull();
        }

        [Fact]
        public void Build_When_NotNull_Returns_Provider()
        {
            new AmazonEBConfigurationSource()                             // Arrange
                .Build(builder: new Mock<IConfigurationBuilder>().Object) // Act
                    .ShouldNotBeNull();                                   // Assert
        }

        #endregion Build
    }
}
