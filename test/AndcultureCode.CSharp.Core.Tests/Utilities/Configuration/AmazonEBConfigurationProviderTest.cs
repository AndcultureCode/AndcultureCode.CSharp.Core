using Shouldly;
using System;
using System.IO;
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
using Newtonsoft.Json;
using AndcultureCode.CSharp.Testing.Tests;

namespace AndcultureCode.CSharp.Core.Tests.Unit.Utilities.Configuration
{
    /// <summary>
    /// Test fixture MUST NOT be run in parallel
    /// </summary>
    public class AndcultureEBConfigurationProviderTest : CoreUnitTest
    {
        #region Setup

        public AndcultureEBConfigurationProviderTest(ITestOutputHelper output) : base(output) { }

        #endregion Setup


        #region Teardown

        public override void Dispose()
        {
            base.Dispose();
            AmazonEBConfigurationProvider.CachedConfiguration = null;
        }

        #endregion Teardown


        #region Get

        [Fact]
        public void Get_When_Key_DoesNotExist_Returns_Null()
        {
            // Arrange
            var sut = new AmazonEBConfigurationProvider();

            // Act
            var result = sut.Get("notfound");

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Get_When_Key_DoesExist_Returns_ExpectedValue()
        {
            // Arrange
            var expectedKey = Random.String();
            var expectedValue = Random.String();
            var sutMock = new Mock<AmazonEBConfigurationProvider>() { CallBase = true };
            var sut = sutMock.Object;

            sutMock.Setup(e => e.Read()).Returns(new Dictionary<string, string> { { expectedKey, expectedValue } });

            // Act
            var result = sut.Get(expectedKey);

            // Assert
            result.ShouldBe(expectedValue);
        }

        #endregion Get


        #region Has

        [Fact]
        public void Has_When_Key_DoesNotExist_Returns_False()
        {
            // Arrange
            var sut = new AmazonEBConfigurationProvider();

            // Act
            var result = sut.Has("notfound");

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Has_When_Key_DoesExist_Returns_True()
        {
            // Arrange
            var expectedKey = Random.String();
            var expectedValue = Random.String();
            var sutMock = new Mock<AmazonEBConfigurationProvider>() { CallBase = true };
            var sut = sutMock.Object;

            sutMock.Setup(e => e.Read()).Returns(new Dictionary<string, string> { { expectedKey, expectedValue } });

            // Act
            var result = sut.Has(expectedKey);

            // Assert
            result.ShouldBeTrue();
        }

        #endregion Has


        #region Load

        [Fact]
        public void Load_When_Configuration_DoesNotExist_DoesNotThrow()
        {
            // Arrange
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: "file-that-does-not-exist");

            // Act
            var result = Record.Exception(() => sut.Load());

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Load_When_Configuration_Exists_Without_Values_DoesNotThrow()
        {
            // Arrange
            var filePath = $"test-file-{Random.Int()}";
            File.WriteAllText(filePath, "{}");
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: filePath);

            // Act
            var result = Record.Exception(() => sut.Load());

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void Load_When_Configuration_Exists_With_Values_DoesNotThrow()
        {
            // Arrange
            var filePath = $"test-file-{Random.Int()}";
            var testPropertyName = "testProperty";
            var testPropertyValue = Random.Int().ToString();
            var expected = new
            {
                iis = new
                {
                    env = new string[]
                    {
                        $"{testPropertyName}={testPropertyValue}"
                    }
                }
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(expected, Formatting.Indented));
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: filePath);

            // Act
            var result = Record.Exception(() => sut.Load());

            // Assert
            result.ShouldBeNull();
        }

        #endregion Load


        #region Read

        [Fact]
        public void Read_When_CachedConfiguration_Set_Returns_CachedConfiguration()
        {
            // Arrange
            var expected = new Dictionary<string, string>();
            var sut = new AmazonEBConfigurationProvider();
            AmazonEBConfigurationProvider.CachedConfiguration = expected;

            // Act
            var result = sut.Read();

            // Assert
            result.ShouldBe(expected);
        }

        [Fact]
        public void Read_When_ConfigurationFilePath_DoesNotExist_Returns_Empty_Dictionary()
        {
            // Arrange
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: "/file-that-does-not-exist");

            // Act
            var result = sut.Read();

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void Read_When_ConfigurationFilePath_InvalidJson_Returns_Empty_Dictionary()
        {
            // Arrange
            var filePath = $"test-file-{Random.Int()}";
            File.WriteAllText(filePath, "<invalid>xml</invalid>");
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: filePath);

            // Act
            var result = sut.Read();

            // Assert
            File.Delete(filePath);
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void Read_When_ConfigurationFilePath_Without_Env_Variable_Returns_Empty_Dictionary()
        {
            // Arrange
            var filePath = $"test-file-{Random.Int()}";
            File.WriteAllText(filePath, "{}");
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: filePath);

            // Act
            var result = sut.Read();

            // Assert
            File.Delete(filePath);
            result.ShouldNotBeNull();
            result.ShouldBeEmpty();
        }

        [Fact]
        public void Read_When_ConfigurationFilePath_With_Env_Variable_Returns_Dictionary_With_Value()
        {
            // Arrange
            var filePath = $"test-file-{Random.Int()}";
            var testPropertyName = "testProperty";
            var testPropertyNameTwo = "testPropertyTwo";
            var testPropertyValue = Random.Int().ToString();
            var testPropertyValueTwo = Random.Int().ToString();
            var expected = new
            {
                iis = new
                {
                    env = new string[]
                    {
                        $"{testPropertyName}={testPropertyValue}",
                        $"{testPropertyNameTwo}={testPropertyValueTwo}"
                    }
                }
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(expected, Formatting.Indented));
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: filePath);

            // Act
            var result = sut.Read();

            // Assert
            File.Delete(filePath);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result[testPropertyName].ShouldBe(testPropertyValue);
            result[testPropertyNameTwo].ShouldBe(testPropertyValueTwo);
        }

        [Fact]
        public void Read_When_ConfigurationFilePath_With_Env_Variable_Containing_Double_Underscores_Returns_Dictionary_With_Value_Containing_Colon()
        {
            // Arrange
            var filePath = $"test-file-{Random.Int()}";
            var testPropertyName = "test__Property";
            var testPropertyNameTwo = "test__Property__Two";
            var testPropertyValue = Random.Int().ToString();
            var testPropertyValueTwo = Random.Int().ToString();
            var expected = new
            {
                iis = new
                {
                    env = new string[]
                    {
                        $"{testPropertyName}={testPropertyValue}",
                        $"{testPropertyNameTwo}={testPropertyValueTwo}"
                    }
                }
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(expected, Formatting.Indented));
            var sut = new AmazonEBConfigurationProvider(configurationFilePath: filePath);

            // Act
            var result = sut.Read();

            // Assert
            File.Delete(filePath);
            result.ShouldNotBeNull();
            result.ShouldNotBeEmpty();
            result[testPropertyName.Replace("__", ":")].ShouldBe(testPropertyValue);
            result[testPropertyNameTwo.Replace("__", ":")].ShouldBe(testPropertyValueTwo);
        }

        #endregion Read


    }
}
