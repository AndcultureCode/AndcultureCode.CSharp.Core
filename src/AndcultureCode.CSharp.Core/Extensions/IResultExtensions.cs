using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Models;
using AndcultureCode.CSharp.Extensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace AndcultureCode.CSharp.Core.Extensions
{
    public static class IResultExtensions
    {
        #region AddError

        /// <summary>
        /// Add translated error record of type Error
        /// </summary>
        /// <param name="result"></param>
        /// <param name="localizer"></param>
        /// <param name="key">Error key found in culture files</param>
        /// <param name="arguments">The values with which to format the translated error message</param>
        public static IError AddError<T>(this IResult<T> result, IStringLocalizer localizer, string key, params object[] arguments)
            => result.AddError(key, localizer[key, arguments]);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IError AddError<T>(this IResult<T> result, IError error)
        {
            if (result.Errors == null)
            {
                result.Errors = new List<IError>();
            }

            if (error != null)
            {
                result.Errors.Add(error);
            }

            return error;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="errorType"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IError AddError<T>(this IResult<T> result, ErrorType errorType, string key, string message)
        {
            if (result.Errors == null)
            {
                result.Errors = new List<IError>();
            }

            var error = new Error
            {
                ErrorType = errorType,
                Key = key,
                Message = message
            };

            result.Errors.Add(error);

            return error;
        }

        /// <summary>
        ///  Add translated error record
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="localizer"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static IError AddError<T>(this IResult<T> result, IStringLocalizer localizer, ErrorType type, string key, params object[] arguments)
            => result.AddError(type, key, localizer[key, arguments]);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IError AddError<T>(this IResult<T> result, string key, string message) => result.AddError(ErrorType.Error, key, message);

        /// <summary>
        /// Adds a new error with the calling class and method name as the key
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IError AddError<T>(this IResult<T> result, string message)
        {
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var className = method.DeclaringType.Name;
            var methodName = Regex.Replace(method.Name, "<|>.*", ""); // Method name without wrapper
            var key = $"{className}.{methodName}";

            return result.AddError(ErrorType.Error, key, message);
        }

        #endregion AddError

        #region AddExceptionError

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static IError AddExceptionError<T>(this IResult<T> result, string key, Exception exception)
        {
            var message = $"{exception.Message}\n{exception.StackTrace}";
            return result.AddError(ErrorType.Error, key, message);
        }

        #endregion AddExceptionError

        #region AddValidationError

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IError AddValidationError<T>(this IResult<T> result, string key, string message) => result.AddError(ErrorType.ValidationError, key, message);

        /// <summary>
        /// Add translated error record of type Validation
        /// </summary>
        /// <param name="result"></param>
        /// <param name="localizer"></param>
        /// <param name="key">Error key found in culture files</param>
        /// <param name="arguments">The values with which to format the translated error message</param>
        public static IError AddValidationError<T>(this IResult<T> result, IStringLocalizer localizer, string key, params object[] arguments)
            => result.AddValidationError(key, localizer[key, arguments]);

        #endregion AddValidationError

        #region AddErrors

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static List<IError> AddErrors<T>(this IResult<T> result, IEnumerable<IError> errors)
        {
            if (errors == null || !errors.Any())
            {
                return result.Errors;
            }

            foreach (var error in errors)
            {
                result.AddError(error);
            }

            return result.Errors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<IError> AddErrors<T, TSource>(this IResult<T> destination, IResult<TSource> source)
        {
            destination.AddErrors(source?.Errors);
            return destination.Errors;
        }

        /// <summary>
        /// Convenience method to simplify common scenario of bubbling errors and
        /// returning null (or default for T type) to the parent
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static T AddErrorsAndReturnDefault<T, TSource>(this IResult<T> destination, IResult<TSource> source)
        {
            destination.AddErrors(source?.Errors);
            return default(T);
        }

        #endregion AddErrors

        #region AddErrorsAndLog

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="errorKey"></param>
        /// <param name="errorMessage"></param>
        /// <param name="resourceIdentifier"></param>
        public static void AddErrorAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, long? resourceIdentifier = null)
        {
            var methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            var logMessage = errorMessage;
            result.AddErrorsAndLog<T>(logger, errorKey, errorMessage, logMessage, resourceIdentifier?.ToString(), null, methodName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="errorKey"></param>
        /// <param name="errorMessage"></param>
        /// <param name="resourceIdentifier"></param>
        /// <param name="errors"></param>
        public static void AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, long resourceIdentifier, IEnumerable<IError> errors = null)
        {
            var methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            var logMessage = errorMessage;
            result.AddErrorsAndLog<T>(logger, errorKey, errorMessage, logMessage, resourceIdentifier.ToString(), errors, methodName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="errorKey"></param>
        /// <param name="errorMessage"></param>
        /// <param name="errors"></param>
        public static void AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, IEnumerable<IError> errors = null)
        {
            var methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            var logMessage = errorMessage;
            result.AddErrorsAndLog<T>(logger, errorKey, errorMessage, logMessage, null, errors, methodName);
        }

        /// <summary>
        /// Add error record and log message
        /// </summary>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="errorKey">Error key found in culture files</param>
        /// <param name="errorMessage">Translated error message</param>
        /// <param name="logMessage">Log message - commonly un-translated version of errorMessage</param>
        /// <param name="resourceIdentifier"></param>
        /// <param name="errors">Additional errors to forward. These are assumed to have already been translated.</param>
        /// <param name="methodName">Name of calling method for use in log message for improved debugging</param>
        public static void AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, string errorKey, string errorMessage, string logMessage, string resourceIdentifier, IEnumerable<IError> errors = null, string methodName = null)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                errorMessage = "";
            }

            // Add singular error
            if (!string.IsNullOrWhiteSpace(errorKey))
            {
                result.AddError(errorKey, errorMessage);
            }

            // Add error list
            if (errors != null && errors.Any())
            {
                result.AddErrors(errors);
            }

            var identifierText = !string.IsNullOrWhiteSpace(resourceIdentifier) ? $" ({resourceIdentifier}) -" : "";

            // If not provided, try to detect method name
            if (string.IsNullOrWhiteSpace(methodName))
            {
                methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            }

            // Log all of the above
            logger.LogError($"[{methodName}]{identifierText} {logMessage}");
        }

        /// <summary>
        /// Add translated error record and log un-translated message
        /// </summary>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="localizer"></param>
        /// <param name="errorKey">Error key found in culture files</param>
        /// <param name="arguments">The values with which to format the translated error message</param>
        public static void AddErrorAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, params object[] arguments)
        {
            var errorMessage = localizer[errorKey, arguments];
            var logMessage = localizer.Default(errorKey, arguments);
            var methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            result.AddErrorsAndLog<T>(logger, errorKey, errorMessage, logMessage, null, null, methodName);
        }

        /// <summary>
        /// Add translated error record and log un-translated message
        /// </summary>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="localizer"></param>
        /// <param name="errorKey">Error key found in culture files</param>
        /// <param name="resourceIdentifier"></param>
        /// <param name="arguments">The values with which to format the translated error message</param>
        public static void AddErrorAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, long resourceIdentifier, params object[] arguments)
        {
            var errorMessage = localizer[errorKey, arguments];
            var logMessage = localizer.Default(errorKey, arguments);
            var methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            result.AddErrorsAndLog<T>(logger, errorKey, errorMessage, logMessage, resourceIdentifier.ToString(), null, methodName);
        }

        /// <summary>
        /// Add translated error record and log un-translated message
        /// </summary>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="localizer"></param>
        /// <param name="errorKey">Error key found in culture files</param>
        /// <param name="resourceIdentifier"></param>
        /// <param name="errors">Additional errors to forward. These are assumed to have already been translated.</param>
        /// <param name="arguments">The values with which to format the translated error message</param>
        public static void AddErrorsAndLog<T>(this IResult<T> result, ILogger logger, IStringLocalizer localizer, string errorKey, long resourceIdentifier, IEnumerable<IError> errors = null, params object[] arguments)
        {
            var errorMessage = localizer[errorKey, arguments];
            var logMessage = localizer.Default(errorKey, arguments);
            var methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            result.AddErrorsAndLog<T>(logger, errorKey, errorMessage, logMessage, resourceIdentifier.ToString(), errors, methodName);
        }

        #endregion AddErrorsAndLog

        #region AddNextLinkParam

        /// <summary>
        /// Adds a new key value pair to the NextLinkParams. Essentially saves needing to
        /// null check and externally manage NextLinkParams property directly.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="key">Unique key to add for next link params</param>
        /// <param name="value">Value for supplied key. Can be null</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Reference to NextLinkParams</returns>
        public static Dictionary<string, string> AddNextLinkParam<T>(this IResult<T> result, string key, string value)
        {
            if (result == null)
            {
                return null;
            }

            if (result.NextLinkParams == null)
            {
                result.NextLinkParams = new Dictionary<string, string>();
            }

            result.NextLinkParams.Add(key, value);

            return result.NextLinkParams;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, string> AddNextLinkParam<T>(this IResult<T> result, string key, int value) => result.AddNextLinkParam(key, value.ToString());

        #endregion AddNextLinkParam

        #region AddNextLinkParams

        /// <summary>
        /// Adds NextLinkParams key value pairs from a source IResult to a destination IResult.
        ///
        /// The destination result's NextLinkParams dictionary reference will remain unchanged.
        /// </summary>
        /// <param name="destinationResult">IResult destination to which next link params are copied</param>
        /// <param name="sourceResult">IResult source from which to copy next link params</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>Reference to NextLinkParams</returns>
        public static Dictionary<string, string> AddNextLinkParams<T>(this IResult<T> destinationResult, IResult<T> sourceResult)
        {
            if (destinationResult == null)
            {
                return null;
            }

            if (sourceResult == null || sourceResult.NextLinkParams == null)
            {
                return destinationResult.NextLinkParams;
            }

            foreach (var pair in sourceResult.NextLinkParams)
            {
                destinationResult.AddNextLinkParam(pair.Key, pair.Value);
            }

            return destinationResult.NextLinkParams;
        }

        #endregion AddNextLinkParams

        #region DoesNotHaveErrors

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public static bool DoesNotHaveErrors<T>(this IResult<T> result, ErrorType errorType) => !result.HasErrors(errorType);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DoesNotHaveErrors<T>(this IResult<T> result, string key) => !result.HasErrors(key);

        #endregion DoesNotHaveErrors

        #region GetErrors

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public static List<IError> GetErrors<T>(this IResult<T> result, ErrorType errorType)
        {
            if (!result.HasErrors(errorType))
            {
                return null;
            }

            return result.Errors.Where(e => e.ErrorType == errorType).ToList();
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<IError> GetErrors<T>(this IResult<T> result, string key)
        {
            if (!result.HasErrors(key))
            {
                return null;
            }

            return result.Errors.Where(e => e.Key == key).ToList();
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static List<IError> GetValidationErrors<T>(this IResult<T> result) => result.GetErrors(ErrorType.ValidationError);

        #endregion GetErrors

        #region HasErrors

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultList"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public static bool HasErrors<T>(this IEnumerable<IResult<T>> resultList, ErrorType errorType)
            => resultList != null && resultList.Any(r => r.HasErrors(errorType));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultList"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasErrors<T>(this IEnumerable<IResult<T>> resultList, string key)
            => resultList != null && resultList.Any(r => r.HasErrors(key));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public static bool HasErrors<T>(this IEnumerable<IResult<T>> resultList)
            => resultList != null && resultList.Any(r => r.HasErrors);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="errorType"></param>
        /// <returns></returns>
        public static bool HasErrors<T>(this IResult<T> result, ErrorType errorType)
            => result.HasErrors && result.Errors.Any(e => e.ErrorType == errorType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasErrors<T>(this IResult<T> result, string key)
            => result.HasErrors && result.Errors.Any(e => e.Key == key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool HasErrorsOrResultIsFalse(this IResult<bool> result)
            => result.HasErrors || !result.ResultObject;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool HasErrorsOrResultIsNull<T>(this IResult<T> result)
            => result.HasErrors || result.ResultObject == null;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool HasValidationErrors<T>(this IResult<T> result)
            => result.HasErrors(ErrorType.ValidationError);

        #endregion HasErrors

        #region ListErrors

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultList"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ListErrors<T>(this IEnumerable<IResult<T>> resultList, string delimiter = "\n")
        {
            if (resultList.IsNullOrEmpty())
            {
                return null;
            }

            var resultsWithErrors = resultList.Where(e => e.HasErrors);
            if (resultsWithErrors.IsEmpty())
            {
                return null;
            }

            return resultsWithErrors.Select(e => e.ListErrors(delimiter)).Join(delimiter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ListErrors<T>(this IResult<T> result, string delimiter = "\n")
        {
            if (!result.HasErrors)
            {
                return null;
            }

            delimiter = string.IsNullOrEmpty(delimiter) ? "\n" : delimiter;
            var list = new List<string>();

            foreach (var error in result.Errors)
            {
                list.Add($"{error.Key}: {error.Message}");
            }

            return string.Join(delimiter, list);
        }

        #endregion ListErrors

        #region ThrowIfAnyErrors

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="customException"></param>
        /// <returns></returns>
        public static IResult<T> ThrowIfAnyErrors<T>(this IResult<T> result, Exception customException = null)
        {
            if (!result.HasErrors)
            {
                return result;
            }

            throw customException == null ? new Exception(result.ListErrors()) : customException;
        }

        #endregion ThrowIfAnyErrors

        #region ThrowIfAnyErrorsOrResultIsNull

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="hasErrorsException"></param>
        /// <param name="resultNullException"></param>
        /// <returns></returns>
        public static IResult<T> ThrowIfAnyErrorsOrResultIsNull<T>(
            this IResult<T> result,
            Exception hasErrorsException = null,
            Exception resultNullException = null)
        {
            if (result.HasErrors)
            {
                throw hasErrorsException == null ? new Exception(result.ListErrors()) :
                                                      hasErrorsException;
            }

            if (result.ResultObject == null)
            {
                throw resultNullException == null ? new Exception($"Result object for IResult returning {typeof(T).Name} is null!") :
                                                      resultNullException;
            }

            return result;
        }

        #endregion ThrowIfAnyErrorsOrResultIsNull

        #region ThrowIfAnyErrorsOrResultIsFalse

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="hasErrorsException"></param>
        /// <param name="resultNullException"></param>
        /// <returns></returns>
        public static IResult<bool> ThrowIfAnyErrorsOrResultIsFalse(
            this IResult<bool> result,
            Exception hasErrorsException = null,
            Exception resultNullException = null)
        {
            if (result.HasErrors)
            {
                throw hasErrorsException == null ? new Exception(result.ListErrors()) :
                                                   hasErrorsException;
            }

            if (!result.ResultObject)
            {
                throw resultNullException == null ? new Exception("Result object for IResult is false") :
                                                    resultNullException;
            }

            return result;
        }

        #endregion ThrowIfAnyErrorsOrResultIsFalse
    }
}
