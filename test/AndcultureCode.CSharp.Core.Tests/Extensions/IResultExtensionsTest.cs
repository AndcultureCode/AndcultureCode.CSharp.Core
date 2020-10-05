using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models.Errors;
using AndcultureCode.CSharp.Extensions;
using AndcultureCode.CSharp.Testing.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace AndcultureCode.CSharp.Core.Tests.Unit.Extensions
{
    public class IResultExtensionsTest : CoreUnitTest
    {

        #region Setup

        public IResultExtensionsTest(ITestOutputHelper output) : base(output) { }

        #endregion Setup

        #region AddError(IError)

        [Fact]
        public void AddError_IError_Overload_When_Result_Errors_IsNull_Result_Contains_Error()
        {
            // Arrange
            var sut = new Result<bool>();
            sut.Errors = null; // <------------ while defaulted, ensuring null in setup
            var expected = new Error();

            // Act
            var result = sut.AddError(error: expected);

            // Assert
            sut.Errors.ShouldNotBeNull();
            sut.Errors.ShouldContain(expected);
        }

        [Fact]
        public void AddError_IError_Overload_When_Result_Errors_IsNull_Returns_Error()
        {
            // Arrange
            var sut = new Result<bool>();
            sut.Errors = null; // <------------ while defaulted, ensuring null in setup
            var expected = new Error();

            // Act
            var result = sut.AddError(error: expected);

            // Assert
            result.ShouldBe(expected);
        }

        #endregion AddError(IError)

        #region AddError(message)

        [Fact]
        public void AddError_MessageOnly_Overload_Adds_Error_With_Supplied_Message()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedMessage = Random.String();

            // Act
            destinationResult.AddError(expectedMessage);

            // Assert
            destinationResult.Errors.ShouldContain(e => e.Message == expectedMessage);
        }

        [Fact]
        public void AddError_MessageOnly_Overload_Adds_Error_With_Key_Containing_Caller_ClassName()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedClassName = this.GetType().Name;

            // Act
            destinationResult.AddError(Random.String());

            // Assert
            destinationResult.Errors.ShouldContain(e => e.Key.StartsWith(expectedClassName), $"Expected to startWith the class name of '{expectedClassName}', but it did not");
        }

        [Fact]
        public void AddError_MessageOnly_Overload_Adds_Error_With_Key_Containing_Caller_MethodName()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedMethodName = "AddError_MessageOnly_Overload_Adds_Error_With_Key_Containing_Caller_MethodName";

            // Act
            destinationResult.AddError(Random.String());

            // Assert
            destinationResult.Errors.ShouldContain(e => e.Key.EndsWith(expectedMethodName), $"Expected to endWith the method name of '{expectedMethodName}', but it did not");
        }

        [Fact]
        public IResult<bool> AddError_MessageOnly_Overload_When_Within_DoTry_Adds_Error_With_Key_Containing_Caller_ClassName() => Do<bool>.Try((r) =>
        {

            // Arrange
            var destinationResult = new Result<object>();
            var expectedClassName = typeof(IResultExtensionsTest).Name;

            // Act
            destinationResult.AddError(Random.String());

            // Assert
            destinationResult.Errors.ShouldContain(e => e.Key.StartsWith(expectedClassName), $"Expected to startWith the class name of '{expectedClassName}', but it did not");

            return true;

        }).Catch<Exception>((ex, r) =>
        {
            throw ex; // re-throw for test runner
        }).Result;

        [Fact]
        public IResult<bool> AddError_MessageOnly_Overload_When_Within_DoTry_Adds_Error_With_Key_Containing_Caller_MethodName() => Do<bool>.Try((r) =>
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedMethodName = "AddError_MessageOnly_Overload_When_Within_DoTry_Adds_Error_With_Key_Containing_Caller_MethodName";

            // Act
            destinationResult.AddError(Random.String());

            // Assert
            destinationResult.Errors.ShouldContain(e => e.Key.EndsWith(expectedMethodName), $"Expected to endWith the method name of '{expectedMethodName}', but it did not");

            return true;

        }).Catch<Exception>((ex, r) =>
        {
            throw ex; // re-throw for test runner
        }).Result;

        #endregion AddError(message)

        #region AddError(localizer, type, key, arguments)

        [Fact]
        public void AddError_Localizer_And_ErrorType_Overload_When_No_Translation_Exists_Adds_Untranslated_Error()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var expectedType = new List<ErrorType> { ErrorType.Error, ErrorType.ValidationError }.PickRandom();
            var localizer = Mock.Of<IStringLocalizer>();

            // Act
            destinationResult.AddError(localizer, expectedType, expectedKey);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == expectedType &&
                e.Key == expectedKey &&
                e.Message == null // <---- added message with null
            );
        }

        [Fact]
        public void AddError_Localizer_And_ErrorType_Overload_Adds_Translated_Error()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var expectedType = new List<ErrorType> { ErrorType.Error, ErrorType.ValidationError }.PickRandom();

            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);

            // Act
            destinationResult.AddError(localizer, expectedType, expectedKey);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == expectedType &&
                e.Key == expectedKey &&
                e.Message == expectedMessage
            );
        }

        #endregion AddError(localizer, type, key, arguments)

        #region AddError(localizer, key, arguments)

        [Fact]
        public void AddError_Localizer_Without_ErrorType_Overload_When_No_Translation_Exists_Adds_Untranslated_Error()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var localizer = Mock.Of<IStringLocalizer>();

            // Act
            destinationResult.AddError(localizer, expectedKey);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.Error && // <---- Error
                e.Key == expectedKey &&
                e.Message == null // <---- added message with null
            );
        }

        [Fact]
        public void AddError_Localizer_Without_ErrorType_Overload_Adds_Translated_Error()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();

            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);

            // Act
            destinationResult.AddError(localizer, expectedKey);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.Error && // <---- Error
                e.Key == expectedKey &&
                e.Message == expectedMessage
            );
        }

        #endregion AddError(localizer, key, arguments)

        #region AddErrors(source)

        [Fact]
        public void
            AddErrors_Source_Overload_When_Source_Errors_IsNull_Then_Returns_Destination_Errors_Without_Any_Additions()
        {
            // Arrange
            var expected = new Error();
            var sut = BuildResult<bool>(e => e.Errors = new List<IError> { expected });
            var source = new Result<string>();

            // Act
            var result = sut.AddErrors(source);

            // Assert
            result.Count.ShouldBe(1);
            result.ShouldContain(expected);
        }

        [Fact]
        public void AddErrors_Source_Overload_When_Source_Has_Errors_Then_Returns_Errors_From_Destination_And_Source()
        {
            // Arrange
            var expected1 = new Error();
            var expected2 = new Error();
            var sut = new Result<bool>();
            sut.Errors = new List<IError> { expected1 };
            var source = new Result<string>();
            source.Errors = new List<IError> { expected2 };

            // Act
            var result = sut.AddErrors(source);

            // Assert
            result.Count.ShouldBe(2);
            result.ShouldContain(expected1);
            result.ShouldContain(expected2);
        }

        #endregion AddErrors(source)

        #region AddErrorsAndLog

        // DEVELOPER NOTE: Some of the tests in this region will be testing to see if methods of the logger were called.
        // Checking to see that one method calls another is typically seen as an antipattern but since we don't have any
        // output to test against, the best we can do is test that the logger is called with the expected arguments.

        #region AddErrorAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, long? resourceIdentifier = null)

        [Fact]
        public void AddErrorAndLog_Adds_Error_To_Result()
        {
            // Arrange
            var errorKey = "Error Key";
            var errorMessage = "Error Message";
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorAndLog(mockLogger.Object, errorKey, errorMessage);

            // Assert
            result.Errors.ShouldContain(e => e.Key == errorKey && e.Message == errorMessage);
        }

        [Fact]
        public void AddErrorAndLog_Calls_The_Log_Method_Of_The_Logger_With_Formatted_Message()
        {
            // Arrange
            var errorKey = "ErrorKey";
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();
            var resourceIdentifier = 42;
            var logMessage = "Log Message";

            // Act
            result.AddErrorAndLog(mockLogger.Object, errorKey, logMessage, resourceIdentifier);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        #endregion AddErrorAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, long? resourceIdentifier = null)

        #region AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, long resourceIdentifier, IEnumerable<IError> errors = null)

        [Fact]
        public void AddErrorsAndLog_When_Provided_With_Error_List_Then_Adds_List_To_Result()
        {
            // Arrange
            var errors = BuildList<Error>(2);
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorsAndLog(
                logger: mockLogger.Object,
                errorKey: null,
                errorMessage: null,
                resourceIdentifier: 1,
                errors: errors);

            // Assert
            result.Errors.ShouldContain(errors[0]);
            result.Errors.ShouldContain(errors[1]);
        }

        [Fact]
        public void AddErrorsAndLog_When_Provided_With_Error_Key_Then_Adds_Single_Error_To_Result()
        {
            // Arrange
            var errorKey = "Error Key";
            var errorMessage = "Error Message";
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorsAndLog(mockLogger.Object, errorKey, errorMessage, 1);

            // Assert
            result.Errors.ShouldContain(e => e.Key == errorKey && e.Message == errorMessage);
        }

        [Fact]
        public void AddErrorsAndLog_Calls_The_Log_Method_Of_The_Logger_With_Formatted_Message()
        {
            // Arrange
            var errorKey = "ErrorKey";
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();
            var resourceIdentifier = 42;
            var logMessage = "Log Message";

            // Act
            result.AddErrorsAndLog(mockLogger.Object, errorKey, logMessage, resourceIdentifier);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        #endregion AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, long resourceIdentifier, IEnumerable<IError> errors = null)

        #region AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, IEnumerable<IError> errors = null)

        [Fact]
        public void AddErrorsAndLog_Without_ResourceId_Overload_When_Provided_With_Error_List_Then_Adds_List_To_Result()
        {
            // Arrange
            var errorList = BuildList<Error>(2);
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorsAndLog(
                logger: mockLogger.Object,
                errorKey: null,
                errorMessage: null,
                errors: errorList);

            // Assert
            result.Errors.ShouldContain(errorList[0]);
            result.Errors.ShouldContain(errorList[1]);
        }

        [Fact]
        public void AddErrorsAndLog_Without_ResourceId_Overload_When_Provided_With_Error_Key_Then_Adds_Single_Error_To_Result()
        {
            // Arrange
            var errorKey = "Error Key";
            var errorMessage = "Error Message";
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorsAndLog(mockLogger.Object, errorKey, errorMessage, null);

            // Assert
            result.Errors.ShouldContain(e => e.Key == errorKey && e.Message == errorMessage);
        }

        [Fact]
        public void AddErrorsAndLog_Without_ResourceId_Overload_Calls_The_Log_Method_Of_The_Logger_With_Formatted_Message()
        {
            // Arrange
            var errorKey = "ErrorKey";
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();
            var logMessage = "Log Message";

            // Act
            result.AddErrorsAndLog(mockLogger.Object, errorKey, logMessage, null);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        #endregion AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, IEnumerable<IError> errors = null)

        #region AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, string logMessage, string resourceIdentifier, IEnumerable<IError> errors = null, string methodName = null)

        [Fact]
        public void AddErrorsAndLog_When_Given_Error_Key_Then_Adds_Single_Error_To_Result()
        {
            // Arrange
            var errorKey = "Error Key";
            var errorMessage = "Error Message";
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorsAndLog(mockLogger.Object, errorKey, errorMessage, "Log Message", "ResourceId");

            // Assert
            result.Errors.ShouldContain(e => e.Key == errorKey && e.Message == errorMessage);
        }

        [Fact]
        public void AddErrorsAndLog_When_Given_Error_List_Then_Adds_List_To_Result()
        {
            // Arrange
            var errorList = BuildList<Error>(2);
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorsAndLog(mockLogger.Object, null, null, null, null, errorList);

            // Assert
            result.Errors.ShouldContain(errorList[0]);
            result.Errors.ShouldContain(errorList[1]);
        }

        [Fact]
        public void AddErrorsAndLog_Calls_Log_Method_Of_The_Logger_With_Formatted_Message()
        {
            // Arrange
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();
            var methodName = "AddErrorsAndLog_Calls_LogError_Method_Of_The_Logger";
            var resourceIdentifier = "ResourceId";
            var logMessage = "Log Message";

            // Act
            result.AddErrorsAndLog(mockLogger.Object, null, null, logMessage, resourceIdentifier, null, methodName);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        [Fact]
        public void
            AddErrorsAndLog_When_MethodName_Is_Not_Provided_Then_Retrieves_MethodName_And_Adds_It_To_The_Formatted_Message()
        {
            // Arrange
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();
            var resourceIdentifier = "ResourceId";
            var logMessage = "Log Message";

            // Act
            result.AddErrorsAndLog(mockLogger.Object, null, null, logMessage, resourceIdentifier, null);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        #endregion AddErrorsAndLog

        #region AddErrorAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, params object[] arguments)

        [Fact]
        public void
            AddErrorAndLog_Localizer_Without_ResourceId_Overload_When_No_Translation_Exists_Then_Adds_Error_Without_Message()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var localizer = Mock.Of<IStringLocalizer>();
            var mockLogger = Mock.Of<ILogger>();

            // Act
            destinationResult.AddErrorAndLog(mockLogger, localizer, expectedKey, expectedMessage);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.Error && // <---- Error
                e.Key == expectedKey &&
                e.Message == String.Empty
            );
        }

        [Fact]
        public void AddErrorAndLog_Localizer_Without_ResourceId_Overload_Adds_Translated_Error()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = "expected key";
            var expectedMessage = "expected message";
            var translatedMessage = "translated message";
            var mockLogger = Mock.Of<ILogger>();
            var localizedString = new LocalizedString(expectedKey, translatedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);

            // Act
            destinationResult.AddErrorAndLog(mockLogger, localizer, expectedKey, expectedMessage);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.Error && // <---- Error
                e.Key == expectedKey &&
                e.Message == translatedMessage
            );
        }

        [Fact]
        public void
            AddErrorAndLog_Localizer_Without_ResourceId_Calls_The_Log_Method_Of_The_Logger_With_Formatted_Message()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var mockLogger = new Mock<ILogger>();
            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);

            // Act
            destinationResult.AddErrorAndLog(mockLogger.Object, localizer, expectedKey, expectedMessage);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        #endregion AddErrorAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, params object[] arguments)

        #region AddErrorAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, long resourceIdentifier, params object[] arguments)

        [Fact]
        public void
            AddErrorAndLog_Localizer_With_ResourceId_Overload_When_No_Translation_Exists_Then_Adds_Error_Without_Message()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var localizer = Mock.Of<IStringLocalizer>();
            var mockLogger = Mock.Of<ILogger>();
            var resourceId = 42;

            // Act
            destinationResult.AddErrorAndLog(mockLogger, localizer, expectedKey, resourceId, expectedMessage);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
               e.ErrorType == ErrorType.Error && // <---- Error
               e.Key == expectedKey &&
               e.Message == String.Empty
            );
        }

        [Fact]
        public void AddErrorAndLog_Localizer_With_ResourceId_Overload_Adds_Translated_Error()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var mockLogger = Mock.Of<ILogger>();
            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);
            var resourceId = 42;

            // Act
            destinationResult.AddErrorAndLog(mockLogger, localizer, expectedKey, resourceId, expectedMessage);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.Error && // <---- Error
                e.Key == expectedKey &&
                e.Message == expectedMessage
            );
        }

        [Fact]
        public void
            AddErrorAndLog_Localizer_With_ResourceId_Calls_The_Log_Method_Of_The_Logger_With_Formatted_Message()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var resourceId = 42;
            var mockLogger = new Mock<ILogger>();
            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);

            // Act
            destinationResult.AddErrorAndLog(mockLogger.Object, localizer, expectedKey, resourceId, expectedMessage);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        #endregion AddErrorAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, long resourceIdentifier, params object[] arguments)

        #region AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, long resourceIdentifier, IEnumerable<IError> errors = null, params object[] arguments)

        [Fact]
        public void AddErrorsAndLog_Localizer_With_Errors_Overload_When_Provided_With_Error_List_Then_Adds_List_To_Result()
        {
            // Arrange
            var errorList = BuildList<Error>(2);
            var localizer = Mock.Of<IStringLocalizer>();
            var result = new Result<bool>();
            var mockLogger = new Mock<ILogger>();

            // Act
            result.AddErrorsAndLog(
                logger: mockLogger.Object,
                localizer: localizer,
                errorKey: null,
                resourceIdentifier: 1,
                errors: errorList);

            // Assert
            result.Errors.ShouldContain(errorList[0]);
            result.Errors.ShouldContain(errorList[1]);
        }

        [Fact]
        public void
            AddErrorsAndLog_Localizer_With_Errors_Overload_When_No_Translation_Exists_Then_Adds_Error_Without_Message()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var localizer = Mock.Of<IStringLocalizer>();
            var mockLogger = Mock.Of<ILogger>();
            var resourceId = 42;

            // Act
            destinationResult.AddErrorsAndLog(mockLogger, localizer, expectedKey, resourceId, null, expectedMessage);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.Error && // <---- Error
                e.Key == expectedKey &&
                e.Message == String.Empty
            );
        }

        [Fact]
        public void AddErrorsAndLog_Localizer_With_Errors_Overload_Adds_Translated_Error()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var mockLogger = Mock.Of<ILogger>();
            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);
            var resourceId = 42;

            // Act
            destinationResult.AddErrorsAndLog(mockLogger, localizer, expectedKey, resourceId, null, expectedMessage);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.Error && // <---- Error
                e.Key == expectedKey &&
                e.Message == expectedMessage
            );
        }

        [Fact]
        public void
            AddErrorsAndLog_Localizer_With_Errors_Overload_Calls_The_Log_Method_Of_The_Logger_With_Formatted_Message()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var resourceId = 42;
            var mockLogger = new Mock<ILogger>();
            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);

            // Act
            destinationResult.AddErrorsAndLog(mockLogger.Object, localizer, expectedKey, resourceId, null, expectedMessage);

            // Assert
            mockLogger.Verify(
                x => x.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(),
                           It.IsAny<Func<object, Exception, string>>()), Times.Once);
        }

        #endregion AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, long resourceIdentifier, IEnumerable<IError> errors = null, params object[] arguments)

        #endregion AddErrorsAndLog

        #region AddErrorsAndReturnDefault

        [Fact]
        public void AddErrorsAndReturnDefault_When_Destination_HasErrors_Appends_SourceErrors_And_Returns_DefaultForType()
        {
            // Arrange
            var destinationErrorMessage = "destination error message";
            var sourceErrorMessage = "source error message";
            var destinationResult = new Result<int>(destinationErrorMessage);

            // Act
            var result = destinationResult.AddErrorsAndReturnDefault(new Result<string>(sourceErrorMessage));

            // Assert
            result.ShouldBe(0);
            destinationResult.Errors.Count.ShouldBe(2);
            destinationResult.Errors.ShouldContain(e => e.Message == destinationErrorMessage);
            destinationResult.Errors.ShouldContain(e => e.Message == sourceErrorMessage);
        }

        [Fact]
        public void AddErrorsAndReturnDefault_When_Source_Null_DoesNotModify_Destination_And_Returns_DefaultForType()
        {
            // Arrange
            var destinationErrorMessage = "destination error message";
            var destinationResult = new Result<int>(destinationErrorMessage);

            // Act
            var result = destinationResult.AddErrorsAndReturnDefault((IResult<string>)null);

            // Assert
            result.ShouldBe(0);
            destinationResult.Errors.Count.ShouldBe(1);
            destinationResult.Errors.ShouldContain(e => e.Message == destinationErrorMessage);
        }

        [Fact]
        public void AddErrorsAndReturnDefault_When_Source_DoesNotHaveErrors_DoesNotModify_Destination_And_Returns_DefaultForType()
        {
            // Arrange
            var destinationErrorMessage = "destination error message";
            var destinationResult = new Result<int>(destinationErrorMessage);

            // Act
            var result = destinationResult.AddErrorsAndReturnDefault(new Result<string>());

            // Assert
            result.ShouldBe(0);
            destinationResult.Errors.Count.ShouldBe(1);
            destinationResult.Errors.ShouldContain(e => e.Message == destinationErrorMessage);
        }

        #endregion AddErrorsAndReturnDefault

        #region AddExceptionError

        [Fact]
        public void AddExceptionError_Given_Key_Returns_Error_With_Key()
        {
            // Arrange
            var expectedKey = Random.String();
            var sut = new Result<bool>();

            // Act
            var result = sut.AddExceptionError(expectedKey, new Exception());

            // Assert
            result.ShouldNotBeNull();
            result.Key.ShouldBe(expectedKey);
        }

        [Fact]
        public void AddExceptionError_Given_Exception_Returns_Error_With_Exception_Message()
        {
            // Arrange
            var expectedMessage = Random.String();
            var sut = new Result<bool>();
            var exception = new Exception(expectedMessage);

            // Act
            var result = sut.AddExceptionError(Random.String(), exception);

            // Assert
            result.ShouldNotBeNull();
            result.Message.ShouldContain(expectedMessage);
        }

        [Fact]
        public void AddExceptionError_Appends_Created_Error_To_Result_Errors()
        {
            // Arrange
            var sut = new Result<bool>();
            sut.AddError(key: Random.String(), message: Random.String());

            // Act
            var result = sut.AddExceptionError(Random.String(), new Exception());

            // Assert
            result.ShouldNotBeNull();
            sut.Errors.Count.ShouldBe(2);
            sut.Errors.ShouldContain(result);
        }

        #endregion AddExceptionError

        #region AddNextLinkParam (string, int)

        [Fact]
        public void AddNextLinkParam_ValueAsInt_Overload_When_Result_IsNull_Return_Null()
        {
            IResultExtensions                                                                      // Arrange
                .AddNextLinkParam<object>(result: null, key: Random.String(), value: Random.Int()) // Act
                    .ShouldBeNull();                                                               // Assert
        }

        [Fact]
        public void AddNextLinkParam_ValueAsInt_Overload_When_Result_NextLinkParams_IsNull_Returns_Dictionary()
        {
            new Result<object> { NextLinkParams = null }                     // Arrange
                .AddNextLinkParam(key: Random.String(), value: Random.Int()) // Act
                    .ShouldNotBeNull();                                      // Assert
        }

        [Fact]
        public void AddNextLinkParam_ValueAsInt_Overload_When_Result_NextLinkParams_IsNull_Returns_Reference_To_NextLinkParams()
        {
            // Arrange
            var destinationResult = new Result<object> { NextLinkParams = null };

            // Act
            var result = destinationResult.AddNextLinkParam(key: Random.String(), value: Random.Int());

            // Assert
            result.ShouldBe(destinationResult.NextLinkParams, "Expected to return reference to underlying 'NextLinkParams' property, but did not");
        }

        [Fact]
        public void AddNextLinkParam_ValueAsInt_Overload_When_Given_Key_IsNull_Throws_ArgumentNullException()
        {
            // Arrange
            var destinationResult = new Result<object>();

            // Act
            var result = Record.Exception(() => destinationResult.AddNextLinkParam(key: null, value: Random.Int()));

            // Assert
            result.ShouldBeOfType(typeof(ArgumentNullException));
        }

        [Fact]
        public void AddNextLinkParam_ValueAsInt_Overload_When_Given_Value_IsNull_Returns_Dictionary_With_Entry()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();

            // Act
            var result = destinationResult.AddNextLinkParam(key: expectedKey, value: null);

            // Assert
            result.ShouldContainKeyAndValue(expectedKey, null);
        }

        [Fact]
        public void AddNextLinkParam_ValueAsInt_Overload_When_Given_Value_IsInt_Returns_Dictionary_With_Entry_Converted_To_String()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedValue = Random.Int();

            // Act
            var result = destinationResult.AddNextLinkParam(key: expectedKey, value: expectedValue);

            // Assert
            result.ShouldContainKeyAndValue(expectedKey, expectedValue.ToString());
        }

        #endregion AddNextLinkParam (string, int)

        #region AddNextLinkParam (string, string)

        [Fact]
        public void AddNextLinkParam_When_Result_IsNull_Return_Null()
        {
            IResultExtensions                                                                         // Arrange
                .AddNextLinkParam<object>(result: null, key: Random.String(), value: Random.String()) // Act
                    .ShouldBeNull();                                                                  // Assert
        }

        [Fact]
        public void AddNextLinkParam_When_Result_NextLinkParams_IsNull_Returns_Dictionary()
        {
            new Result<object> { NextLinkParams = null }                        // Arrange
                .AddNextLinkParam(key: Random.String(), value: Random.String()) // Act
                    .ShouldNotBeNull();                                         // Assert
        }

        [Fact]
        public void AddNextLinkParam_When_Result_NextLinkParams_IsNull_Returns_Reference_To_NextLinkParams()
        {
            // Arrange
            var destinationResult = new Result<object> { NextLinkParams = null };

            // Act
            var result = destinationResult.AddNextLinkParam(key: Random.String(), value: Random.String());

            // Assert
            result.ShouldBe(destinationResult.NextLinkParams, "Expected to return reference to underlying 'NextLinkParams' property, but did not");
        }

        [Fact]
        public void AddNextLinkParam_When_Given_Key_IsNull_Throws_ArgumentNullException()
        {
            // Arrange
            var destinationResult = new Result<object>();

            // Act
            var result = Record.Exception(() => destinationResult.AddNextLinkParam(key: null, value: Random.String()));

            // Assert
            result.ShouldBeOfType(typeof(ArgumentNullException));
        }

        [Fact]
        public void AddNextLinkParam_When_Given_Value_IsNull_Returns_Dictionary_With_Entry()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var key = Random.String();

            // Act
            var result = destinationResult.AddNextLinkParam(key: key, value: null);

            // Assert
            result.ShouldContainKeyAndValue(key, null);
        }

        #endregion AddNextLinkParam (string, string)

        #region AddNextLinkParams

        [Fact]
        public void AddNextLinkParams_When_DestinationResult_IsNull_Returns_Null()
        {
            IResultExtensions                                                           // Arrange
                .AddNextLinkParams<object>(destinationResult: null, sourceResult: null) // Act
                    .ShouldBeNull();                                                    // Assert
        }

        [Fact]
        public void AddNextLinkParams_When_DestinationResult_NextLinkParams_IsNull_Given_SourceResult_IsNull_Returns_Null()
        {
            // Arrange
            var destinationResult = new Result<object> { NextLinkParams = null }; // <----------- destination null

            // Act
            var result = destinationResult.AddNextLinkParams(sourceResult: null);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void AddNextLinkParams_When_DestinationResult_NextLinkParams_IsNull_Given_SourceResult_NextListParams_IsNull_Returns_Null()
        {
            // Arrange
            var destinationResult = new Result<object> { NextLinkParams = null }; // <------------ destination null
            var sourceResult = new Result<object> { NextLinkParams = null }; // <------------ source null

            // Act
            var result = destinationResult.AddNextLinkParams(sourceResult: sourceResult);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void AddNextLinkParams_When_DestinationResult_NextLinkParams_IsNull_Given_SourceResult_NextListParams_IsNotEmpty_Returns_Dictionary()
        {
            // Arrange
            var destinationResult = new Result<object> { NextLinkParams = null }; // <----------- destination null

            var sourceResult = new Result<object>();
            sourceResult.AddNextLinkParam(Random.String(), Random.String());

            // Act
            var result = destinationResult.AddNextLinkParams(sourceResult: sourceResult);

            // Assert
            result.ShouldBeOfType(typeof(Dictionary<string, string>));
        }

        [Fact]
        public void AddNextLinkParams_When_DestinationResult_NextLinkParams_IsNull_Given_SourceResult_NextListParams_IsNotEmpty_Returns_Dictionary_With_SourceEntry()
        {
            // Arrange
            var destinationResult = new Result<object> { NextLinkParams = null }; // <----------- destination null
            var sourceTestKey = Random.String();
            var sourceTestValue = Random.String();
            var sourceResult = new Result<object>();
            sourceResult.AddNextLinkParam(sourceTestKey, sourceTestValue);

            // Act
            var result = destinationResult.AddNextLinkParams(sourceResult: sourceResult);

            // Assert
            result.ShouldContainKeyAndValue(sourceTestKey, sourceTestValue);
        }

        [Fact]
        public void AddNextLinkParams_When_DestinationResult_NextLinkParams_HasEntries_Given_SourceResult_NextListParams_HasEntries_Returns_Dictionary_From_DestinationResult()
        {
            // Arrange
            var destinationResult = new Result<object>();
            destinationResult.AddNextLinkParam("testKeyFromDestination", "testValueFromDestination");

            var sourceResult = new Result<object>();
            sourceResult.AddNextLinkParam("testKeyFromSource", "testValueFromSource");

            // Act
            var result = destinationResult.AddNextLinkParams(sourceResult: sourceResult);

            // Assert
            result.ShouldBe(destinationResult.NextLinkParams);
        }

        [Fact]
        public void AddNextLinkParams_When_DestinationResult_NextLinkParams_HasEntries_Given_SourceResult_NextListParams_HasEntries_Returns_Dictionary_With_BothEntries()
        {
            // Arrange
            var destinationTestKey = "testKeyFromDestination";
            var destinationTestValue = "testValueFromDestination";
            var destinationResult = new Result<object>();
            destinationResult.AddNextLinkParam(destinationTestKey, destinationTestValue);

            var sourceTestKey = "testKeyFromSource";
            var sourceTestValue = "testValueFromSource";
            var sourceResult = new Result<object>();
            sourceResult.AddNextLinkParam(sourceTestKey, sourceTestValue);

            // Act
            var result = destinationResult.AddNextLinkParams(sourceResult: sourceResult);

            // Assert
            result.ShouldContainKeyAndValue(destinationTestKey, destinationTestValue);
            result.ShouldContainKeyAndValue(sourceTestKey, sourceTestValue);
        }

        #endregion AddNextLinkParams

        #region AddValidationError(localizer, key, arguments)

        [Fact]
        public void AddValidationError_Localizer_Without_ErrorType_Overload_When_No_Translation_Exists_Adds_Untranslated_ValidationError()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();
            var localizer = Mock.Of<IStringLocalizer>();

            // Act
            destinationResult.AddValidationError(localizer, expectedKey);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.ValidationError && // <---- ValidationError
                e.Key == expectedKey &&
                e.Message == null // <---- added message with null
            );
        }

        [Fact]
        public void AddValidationError_Localizer_Without_ErrorType_Overload_Adds_Translated_ValidationError()
        {
            // Arrange
            var destinationResult = new Result<object>();
            var expectedKey = Random.String();
            var expectedMessage = Random.String();

            var localizedString = new LocalizedString(expectedKey, expectedMessage);
            var localizer = Mock.Of<IStringLocalizer>(e => e[expectedKey, It.IsAny<object[]>()] == localizedString);

            // Act
            destinationResult.AddValidationError(localizer, expectedKey);

            // Assert
            destinationResult.Errors.ShouldContain(e =>
                e.ErrorType == ErrorType.ValidationError && // <---- ValidationError
                e.Key == expectedKey &&
                e.Message == expectedMessage
            );
        }

        #endregion AddValidationError(localizer, key, arguments)

        #region DoesNotHaveErrors<T>(this IResult<T> result, ErrorType errorType)

        [Fact]
        public void DoesNotHaveErrors_ErrorType_Overload_When_There_Are_No_Errors_Returns_True()
        {
            // Arrange
            var sut = new Result<bool>();

            // Act
            var result = sut.DoesNotHaveErrors(ErrorType.Error);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void DoesNotHaveErrors_When_There_Is_An_Error_With_Matching_ErrorType_Then_Returns_False()
        {
            // Arrange
            var error = Build<Error>(e => e.ErrorType = ErrorType.Error);
            var sut = BuildResult<bool>(e => e.Errors = new List<IError> { error });

            // Act
            var result = sut.DoesNotHaveErrors(ErrorType.Error);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void DoesNotHaveErrors_When_There_Is_An_Error_Without_Matching_ErrorType_Then_Returns_True()
        {
            // Arrange
            var error = Build<Error>(e => e.ErrorType = ErrorType.Error);
            var sut = BuildResult<bool>(e => e.Errors = new List<IError> { error });

            // Act
            var result = sut.DoesNotHaveErrors(ErrorType.ValidationError);

            // Assert
            result.ShouldBeTrue();
        }

        #endregion DoesNotHaveErrors<T>(this IResult<T> result, ErrorType errorType)

        #region DoesNotHaveErrors<T>(this IResult<T> result, string key)

        [Fact]
        public void DoesNotHaveErrors_String_Overload_When_There_Are_No_Errors_Returns_True()
        {
            // Arrange
            var sut = new Result<bool>();

            // Act
            var result = sut.DoesNotHaveErrors("ErrorKey");

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void DoesNotHaveErrors_When_There_Is_An_Error_With_Matching_Key_Then_Returns_False()
        {
            // Arrange
            var error = Build<Error>(e => e.ErrorType = ErrorType.Error);
            var sut = BuildResult<bool>(e => e.Errors = new List<IError> { error });

            // Act
            var result = sut.DoesNotHaveErrors(error.Key);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void DoesNotHaveErrors_When_There_Is_An_Error_Without_Matching_Key_Then_Returns_True()
        {
            // Arrange
            var error = Build<Error>(e => e.Key = "ErroryKey");
            var sut = BuildResult<bool>(e => e.Errors = new List<IError> { error });

            // Act
            var result = sut.DoesNotHaveErrors("Key");

            // Assert
            result.ShouldBeTrue();
        }

        #endregion DoesNotHaveErrors<T>(this IResult<T> result, string key)

        #region GetErrors<T>(this IResult<T> result, ErrorType errorType)

        [Fact]
        public void GetErrors_ErrorType_Overload_When_There_Are_No_Errors_Then_Returns_Null()
        {
            // Arrange
            var sut = new Result<bool>();

            // Act
            var result = sut.GetErrors(ErrorType.Error);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GetErrors_When_Errors_Do_Not_Match_Provided_ErrorType_Then_Returns_Null()
        {
            // Arrange
            var error = Build<Error>(e => e.ErrorType = ErrorType.ValidationError);
            var sut = BuildResult<bool>(e => e.Errors = new List<IError> { error });

            // Act
            var result = sut.GetErrors(ErrorType.Error);

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GetErrors_When_ErrorType_Matches_Then_Returns_Errors_With_Matched_Type()
        {
            // Arrange
            var errors = new List<IError>
                {
                    new Error { ErrorType = ErrorType.Error },
                    new Error { ErrorType = ErrorType.Error },
                    new Error { ErrorType = ErrorType.ValidationError }
                };

            var sut = new Result<bool> { Errors = errors };

            // Act
            var result = sut.GetErrors(ErrorType.Error);

            // Assert
            result.Count.ShouldBe(2);
            result.ShouldNotContain(e => e.ErrorType == ErrorType.ValidationError);
        }

        #endregion GetErrors<T>(this IResult<T> result, ErrorType errorType)

        #region GetErrors<T>(this IResult<T> result, string key)

        [Fact]
        public void GetErrors_Key_Overload_When_There_Are_No_Errors_Then_Returns_Null()
        {
            // Arrange
            var sut = new Result<bool>();

            // Act
            var result = sut.GetErrors("ErrorKey");

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GetErrors_When_Error_Key_Does_Not_Match_Provided_Key_Then_Returns_Null()
        {
            // Arrange
            var sut = new Result<bool>
            {
                Errors = new List<IError> { new Error { Key = "ErrorKey" } }
            };

            // Act
            var result = sut.GetErrors("NotErrorKey");

            // Assert
            result.ShouldBeNull();
        }

        [Fact]
        public void GetErrors_When_Error_Key_Matches_Then_Returns_Errors_With_Matched_Key()
        {
            // Arrange
            var errors = new List<IError>
                {
                    new Error { Key = "ErrorKey1" },
                    new Error { Key = "ErrorKey1" },
                    new Error { Key = "ErrorKey2" }
                };

            var sut = new Result<bool> { Errors = errors };

            // Act
            var result = sut.GetErrors("ErrorKey1");

            // Assert
            result.Count.ShouldBe(2);
            result.ShouldNotContain(e => e.Key == "ErrorKey2");
        }

        #endregion GetErrors<T>(this IResult<T> result, string key)

        #region GetValidationErrors

        [Fact]
        public void GetValidationErrors_When_There_Are_Validation_Errors_Then_Returns_List_With_Validation_Errors()
        {
            // Arrange
            var sut = new Result<bool>
            {
                Errors = new List<IError>
                        {
                            new Error { ErrorType = ErrorType.ValidationError },
                            new Error { ErrorType = ErrorType.Error }
                        }
            };

            // Act
            var result = sut.GetValidationErrors();

            // Assert
            result.Count.ShouldBe(1);
            result.ShouldNotContain(e => e.ErrorType == ErrorType.Error);
        }

        [Fact]
        public void GetValidationErrors_When_There_Are_No_Validation_Errors_Then_Returns_Null()
        {
            // Arrange
            var sut = new Result<bool>();

            // Act
            var result = sut.GetValidationErrors();

            // Assert
            result.ShouldBeNull();
        }

        #endregion GetValidationErrors

        #region HasErrors

        #region HasErrors (IEnumerable<IResult<T>>)

        [Fact]
        public void HasErrors_EnumerableList_When_List_IsNull_Returns_False()
        {
            IResultExtensions.HasErrors<bool>(resultList: null).ShouldBeFalse();
        }

        [Fact]
        public void HasErrors_EnumerableList_When_List_IsEmpty_Returns_False()
        {
            new List<IResult<bool>>().HasErrors().ShouldBeFalse();
        }

        [Fact]
        public void HasErrors_EnumerableList_When_List_Without_Errors_Returns_False()
        {
            new List<IResult<bool>> { new Result<bool>() } // Arrange
                .HasErrors()                               // Act
                    .ShouldBeFalse();                      // Assert
        }

        [Fact]
        public void HasErrors_EnumerableList_When_List_With_Errors_Returns_True()
        {
            new List<IResult<bool>> { new Result<bool>("errorkey", "errormessage") } // Arrange
                .HasErrors()                                                         // Act
                    .ShouldBeTrue();                                                 // Assert
        }

        #endregion HasErrors (no arguments)

        #region HasErrors (IEnumerable<IResult<T>>, ErrorType)

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorType_Overload_When_List_IsNull_Returns_False()
        {
            IResultExtensions.HasErrors<bool>(resultList: null, errorType: ErrorType.Error).ShouldBeFalse();
        }

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorType_Overload_When_List_IsEmpty_Returns_False()
        {
            new List<IResult<bool>>().HasErrors(ErrorType.Error).ShouldBeFalse();
        }

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorType_Overload_When_List_Without_Errors_Returns_False()
        {
            new List<IResult<bool>> { new Result<bool>() } // Arrange
                .HasErrors(ErrorType.Error)                // Act
                    .ShouldBeFalse();                      // Assert
        }

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorType_Overload_When_List_With_Errors_Returns_True()
        {
            new List<IResult<bool>> { new Result<bool>("errorkey", "errormessage") } // Arrange
                .HasErrors(ErrorType.Error)                                          // Act
                    .ShouldBeTrue();                                                 // Assert
        }

        #endregion HasErrors (IEnumerable<IResult<T>>, ErrorType)


        #region HasErrors (IEnumerable<IResult<T>>, string)

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorKey_Overload_When_List_IsNull_Returns_False()
        {
            IResultExtensions.HasErrors<bool>(resultList: null, key: Random.String()).ShouldBeFalse();
        }

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorKey_Overload_When_List_IsEmpty_Returns_False()
        {
            new List<IResult<bool>>().HasErrors(key: Random.String()).ShouldBeFalse();
        }

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorKey_Overload_When_List_Without_Errors_Returns_False()
        {
            new List<IResult<bool>> { new Result<bool>() } // Arrange
                .HasErrors(key: Random.String())           // Act
                    .ShouldBeFalse();                      // Assert
        }

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorKey_Overload_When_List_With_Errors_When_Key_DoesNotMatch_Returns_False()
        {
            new List<IResult<bool>> { new Result<bool>("errorkey", "errormessage") } // Arrange
                .HasErrors(key: "404")                                               // Act
                    .ShouldBeFalse();                                                // Assert
        }

        [Fact]
        public void HasErrors_EnumerableList_And_ErrorKey_Overload_When_List_With_Errors_When_Key_Matches_Returns_True()
        {
            new List<IResult<bool>> { new Result<bool>("errorkey", "errormessage") } // Arrange
                .HasErrors(key: "errorkey")                                          // Act
                    .ShouldBeTrue();                                                 // Assert
        }

        #endregion HasErrors (IEnumerable<IResult<T>>, string)

        #endregion HasErrors

        #region HasErrorsOrResultIsFalse

        [Fact]
        public void HasErrorsOrResultIsFalse_When_Has_Errors_Then_Returns_True()
        {
            // Arrange
            var sut = new Result<bool> { Errors = new List<IError> { new Error() } };

            // Act
            var result = sut.HasErrorsOrResultIsFalse();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void HasErrorsOrResultIsFalse_When_Result_Is_False_Then_Returns_True()
        {
            // Arrange
            var sut = new Result<bool> { ResultObject = false };

            // Act
            var result = sut.HasErrorsOrResultIsFalse();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void HasErrorsOrResultIsFalse_When_There_Are_No_Errors_And_Result_Is_Not_False_Then_Returns_False()
        {
            // Arrange
            var sut = new Result<bool> { ResultObject = true };

            // Act
            var result = sut.HasErrorsOrResultIsFalse();

            // Assert
            result.ShouldBeFalse();
        }

        #endregion HasErrorsOrResultIsFalse

        #region HasErrorsOrResultIsNull

        [Fact]
        public void HasErrorsOrResultIsNull_When_Has_Errors_Then_Returns_True()
        {
            // Arrange
            var sut = new Result<bool> { Errors = new List<IError> { new Error() } };

            // Act
            var result = sut.HasErrorsOrResultIsNull();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void HasErrorsOrResultIsNull_When_Result_Is_False_Then_Returns_True()
        {
            // Arrange
            var sut = new Result<string> { ResultObject = null };

            // Act
            var result = sut.HasErrorsOrResultIsNull();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void HasErrorsOrResultIsNull_When_There_Are_No_Errors_And_Result_Is_Not_False_Then_Returns_False()
        {
            // Arrange
            var sut = new Result<bool> { ResultObject = true };

            // Act
            var result = sut.HasErrorsOrResultIsNull();

            // Assert
            result.ShouldBeFalse();
        }

        #endregion HasErrorsOrResultIsNull

        #region HasValidationErrors

        [Fact]
        public void HasValidationErrors_When_There_Are_Errors_With_ValidationError_Type_Then_Returns_True()
        {
            // Arrange
            var sut = new Result<bool>
            {
                Errors = new List<IError> { new Error { ErrorType = ErrorType.ValidationError } }
            };

            // Act
            var result = sut.HasValidationErrors();

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void HasValidationErrors_When_There_Are_Errors_That_Are_Not_Validation_Error_Type_Then_Returns_False()
        {
            // Arrange
            var sut = new Result<bool>
            {
                Errors = new List<IError> { new Error { ErrorType = ErrorType.Error } }
            };

            // Act
            var result = sut.HasValidationErrors();

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void HasValidationErrors_When_There_Are_No_Errors_Then_Returns_False()
        {
            // Arrange
            var sut = new Result<bool>();

            // Act
            var result = sut.HasValidationErrors();

            // Assert
            result.ShouldBeFalse();
        }

        #endregion HasValidationErrors

        #region ListErrors

        #region ListErrors (IEnumerable<IResult<T>>, string)

        [Fact]
        public void ListErrors_When_List_IsNull_Returns_Null()
        {
            IResultExtensions.ListErrors<bool>(resultList: null).ShouldBeNull();
        }

        [Fact]
        public void ListErrors_When_List_IsEmpty_Returns_Null()
        {
            new List<IResult<bool>>().ListErrors().ShouldBeNull();
        }

        [Fact]
        public void ListErrors_When_List_DoesNot_HaveErrors_Returns_Null()
        {
            new List<IResult<bool>> { new Result<bool>() } // Arrange
                .ListErrors()                              // Act
                    .ShouldBeNull();                       // Assert
        }

        [Fact]
        public void ListErrors_When_List_Has_OneError_Returns_Error_Key_And_Message()
        {
            // Arrange
            var sut = new List<IResult<bool>> { new Result<bool>("testkey", "testmessage") };

            // Act
            var result = sut.ListErrors();

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldContain("testkey");
            result.ShouldContain("testmessage");
        }

        [Fact]
        public void ListErrors_When_List_Has_MultipleErrors_Returns_Error_Keys_And_Messages()
        {
            // Arrange
            var sut = new List<IResult<bool>>
                {
                    new Result<bool>("testkey1", "testmessage1"),
                    new Result<bool>("testkey2", "testmessage2")
                };

            // Act
            var result = sut.ListErrors();

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldContain("testkey1");
            result.ShouldContain("testmessage1");
            result.ShouldContain("testkey2");
            result.ShouldContain("testmessage2");
        }

        [Fact]
        public void ListErrors_When_List_Has_MultipleErrors_Given_CustomDelimiter_Returns_Error_Keys_And_Messages_With_Delimiter()
        {
            // Arrange
            var sut = new List<IResult<bool>>
                {
                    new Result<bool>("testkey1", "testmessage1"),
                    new Result<bool>("testkey2", "testmessage2")
                };

            // Act
            var result = sut.ListErrors(delimiter: "delimiter");

            // Assert
            result.ShouldNotBeEmpty();
            result.ShouldContain("testkey1");
            result.ShouldContain("testmessage1");
            result.ShouldContain("testkey2");
            result.ShouldContain("testmessage2");
            result.ShouldContain("delimiter");
        }

        #endregion ListErrors (IEnumerable<IResult<T>>, string)

        #endregion ListErrors

        #region ThrowIfAnyErrors

        [Fact]
        public void ThrowIfAnyErrors_When_No_Errors_Then_Does_Not_Throw_Exception()
        {
            // Arrange
            var result = new Result<bool>();

            // Act && Assert
            Should.NotThrow(() =>
            {
                result.ThrowIfAnyErrors();
            });
        }

        [Fact]
        public void ThrowIfAnyErrors_When_Result_Has_Errors_Then_Throws_Exception()
        {
            // Arrange
            var result = new Result<bool> { Errors = new List<IError> { new Error() } };

            // Act && Assert
            Should.Throw<Exception>(() =>
            {
                result.ThrowIfAnyErrors();
            });
        }

        [Fact]
        public void
            ThrowIfAnyErrors_When_No_Custom_Exception_Is_Provided_Then_Throws_Generic_Exception_With_Error_List()
        {
            // Arrange
            var result = new Result<bool>
            {
                Errors = new List<IError>
                        {
                            new Error { Key = "ErrorKey1", Message = "Error message 1" },
                            new Error { Key = "ErrorKey2", Message = "Error message 2" }
                        }
            };
            var errorList = result.ListErrors();

            // Act && Assert
            var exception = Should.Throw<Exception>(() =>
            {
                result.ThrowIfAnyErrors();
            });

            exception.Message.ShouldBe(errorList);
        }

        [Fact]
        public void ThrowIfAnyErrors_When_Provided_Custom_Exception_Then_Throws_Custom_Exception()
        {
            // Arrange
            var customException = new ArgumentNullException();
            var result = new Result<bool> { Errors = new List<IError> { new Error() } };

            // Act && Assert
            Should.Throw<ArgumentNullException>(() =>
            {
                result.ThrowIfAnyErrors(customException);
            });
        }

        #endregion ThrowIfAnyErrors

        #region ThrowIfAnyErrorsOrResultIsFalse

        [Fact]
        public void ThrowIfAnyErrorsOrResultIsFalse_When_No_Errors_And_Result_Is_True_Then_Returns_Result()
        {
            // Arrange
            var sut = new Result<bool> { ResultObject = true };

            // Act
            var result = sut.ThrowIfAnyErrorsOrResultIsFalse();

            // Assert
            result.ShouldBe(sut);
        }

        [Fact]
        public void ThrowIfAnyErrorsOrResultIsFalse_When_Provided_Exception_For_Errors_Then_Throws_Provided_Exception()
        {
            // Arrange
            var result = new Result<bool> { Errors = new List<IError> { new Error() } };
            var errorException = new ArgumentNullException();

            // Act && Assert
            Should.Throw<ArgumentNullException>(() => { result.ThrowIfAnyErrorsOrResultIsFalse(errorException); });
        }

        [Fact]
        public void ThrowIfAnyErrorsOrResultIsFalse_When_Provided_Exception_For_False_Result_Then_Throws_Provided_Exception()
        {
            // Arrange
            var result = new Result<bool> { ResultObject = false };
            var errorException = new ArgumentNullException();

            // Act && Assert
            Should.Throw<ArgumentNullException>(() => { result.ThrowIfAnyErrorsOrResultIsFalse(null, errorException); });
        }

        [Fact]
        public void ThrowIfAnyErrorsOrResultIsFalse_When_No_Exception_Is_Provided_For_Errors_Then_Throws_Generic_Exception_With_Error_List()
        {
            // Arrange
            var sut = new Result<bool>
            {
                Errors = new List<IError>
                        {
                            new Error { Key = "ErrorKey1", Message = "Error message 1" },
                            new Error { Key = "ErrorKey2", Message = "Error message 2" }
                        }
            };
            var errorList = sut.ListErrors();

            // Act && Assert
            var exception = Should.Throw<Exception>(() => { sut.ThrowIfAnyErrorsOrResultIsFalse(); });

            exception.Message.ShouldBe(errorList);
        }

        [Fact]
        public void
            ThrowIfAnyErrorsOrResultIsFalse_When_No_Exception_Provided_For_False_Result_Then_Throws_Generic_Exception_With_Default_Message()
        {
            // Arrange
            var sut = new Result<bool> { ResultObject = false };
            var defaultMessage = "Result object for IResult is false";

            // Act && Assert
            var exception = Should.Throw<Exception>(() => sut.ThrowIfAnyErrorsOrResultIsFalse());

            exception.Message.ShouldBe(defaultMessage);
        }



        #endregion ThrowIfAnyErrorsOrResultIsFalse

        #region ThrowIfAnyErrorsOrResultIsNull

        [Fact]
        public void ThrowIfAnyErrorsOrResultIsNull_Result_Has_Errors_Throws_Exception()
        {
            var result = new Result<bool?>
            {
                Errors = new List<IError>
                {
                    new Error
                    {
                        Message = "This is a test error!"
                    }
                },
                ResultObject = false
            };
            var thrown = false;

            try
            {
                result.ThrowIfAnyErrorsOrResultIsNull();
            }
            catch (Exception e)
            {
                e.ShouldNotBeNull();
                e.Message.ShouldContain("This is a test error!");
                thrown = true;
            }
            finally
            {
                result.ShouldHaveErrors();
                result.ResultObject.ShouldNotBeNull();
                thrown.ShouldBeTrue();
            }
        }

        [Fact]
        public void ThrowIfAnyErrorsOrResultIsNull_Result_Has_NullResult_And_NoErrors_Throws_Exception()
        {
            var result = new Result<bool?>
            {
                Errors = null,
                ResultObject = null
            };
            var thrown = false;

            try
            {
                result.ThrowIfAnyErrorsOrResultIsNull();
            }
            catch (Exception e)
            {
                e.ShouldNotBeNull();
                e.Message.ShouldContain($"Result object for IResult returning {typeof(bool?).Name} is null!");
                thrown = true;
            }
            finally
            {
                result.ShouldNotHaveErrors();
                result.ResultObject.ShouldBeNull();
                thrown.ShouldBeTrue();
            }
        }

        [Fact]
        public void ThrowIfAnyErrorsOrResultIsNull_Result_IsNotNull_And_DoesNotHaveErrors_DoesNotThrowException()
        {
            var result = new Result<bool?>
            {
                Errors = null,
                ResultObject = true
            };

            result.ThrowIfAnyErrorsOrResultIsNull();

            result.ShouldNotHaveErrors();
            result.ResultObject.ShouldNotBeNull();
        }

        #endregion ThrowIfAnyErrorsOrResultIsNull

    }
}
