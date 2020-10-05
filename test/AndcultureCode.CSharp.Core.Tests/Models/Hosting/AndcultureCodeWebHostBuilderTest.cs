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
using AndcultureCode.CSharp.Core.Models.Hosting;
using AndcultureCode.CSharp.Testing.Tests;

namespace AndcultureCode.CSharp.Core.Tests.Unit.Models.Hosting
{
    public class AndcultureCodeWebHostBuilderTest : CoreUnitTest
    {
        #region Setup

        public AndcultureCodeWebHostBuilderTest(ITestOutputHelper output) : base(output) { }

        #endregion Setup


        #region Constructor (string[] args)

        [Fact]
        public void Constructor_Overload_With_Args_Null_Sets_Args_Null()
        {
            new AndcultureCodeWebHostBuilder(args: null).Args.ShouldBeNull();
        }

        [Fact]
        public void Constructor_Overload_With_Args_NotNull_Sets_Args_To_Provided_Value()
        {
            // Arrange
            var expected = new string[] { Random.String() };

            // Act
            var sut = new AndcultureCodeWebHostBuilder(args: expected);

            // Assert
            sut.Args.ShouldBe(expected);
        }

        #endregion Constructor (string[] args)


        #region CreateDefaultBuilder

        [Fact]
        public void CreateDefaultBuilder_Returns_Builder()
        {
            new AndcultureCodeWebHostBuilder().CreateDefaultBuilder().ShouldNotBeNull();
        }

        #endregion CreateDefaultBuilder
    }
}
