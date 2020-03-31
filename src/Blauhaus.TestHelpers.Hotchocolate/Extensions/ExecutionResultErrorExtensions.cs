using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Blauhaus.Common.Utils.Extensions;
using HotChocolate.Execution;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Blauhaus.Common.TestHelpers.Hotchocolate.Extensions
{
    public static class ExecutionResultErrorExtensions
    {
        public static IExecutionResult VerifyNoErrors(this IExecutionResult executionResult)
        {
            var errors = executionResult.Errors;

            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    Assert.Fail(error.Message);
                }
            }
            return executionResult;
        } 

        public static IExecutionResult VerifyError(this IExecutionResult executionResult, string errorMessage = "")
        {
            var errors = executionResult.Errors;

            if (errors.Count == 0)
            {
                Assert.Fail("No errors were contained in the ExecutionResult");
            }

            var expectedError = errors.FirstOrDefault(x => x.Message == errorMessage);

            if(expectedError == null)
                Assert.Fail($"No errors contained the expected message {errorMessage}");

            return executionResult;
        } 

        public static IExecutionResult VerifyException(this IExecutionResult executionResult, string exceptionErrorMessage)
        {
            var errors = executionResult.Errors;

            if (errors.Count == 0)
            {
                Assert.Fail("No errors were contained in the ExecutionResult");
            }

            var expectedError = errors.FirstOrDefault(x => x.Exception?.Message == exceptionErrorMessage);

            if(expectedError == null)
                Assert.Fail($"No errors contained the expected exception message {exceptionErrorMessage}");
            else
                Assert.That(exceptionErrorMessage, Is.EqualTo(exceptionErrorMessage));

            return executionResult;
        } 

        public static IExecutionResult VerifyException<TException>(this IExecutionResult executionResult, string exceptionErrorMessage = "")
        {
            var errors = executionResult.Errors;

            if (errors.Count == 0)
            {
                Assert.Fail("No errors were contained in the ExecutionResult");
            }

            var expectedError = errors.FirstOrDefault(x => x.Exception.GetType() == typeof(TException));

            if(expectedError == null)
                Assert.Fail($"No errors contained the expected exception message {exceptionErrorMessage}");
            else
            {
                if (string.IsNullOrEmpty(exceptionErrorMessage))
                {
                    Assert.That(expectedError, Is.Not.Null);
                }
                else
                    Assert.That(exceptionErrorMessage, Is.EqualTo(exceptionErrorMessage));
            }

            return executionResult;
        } 

        public static void VerifyErrorCode(this IExecutionResult executionResult, string code)
        {
            
            var errors = executionResult.Errors;
            if (errors.Count == 0)
            {
                Assert.Fail("No errors were found in the execution result");
            }

            var foundError = errors.FirstOrDefault(x => x.Code == code);
            if (foundError != null)
            {
                Assert.That(foundError.Code, Is.EqualTo(code));
            }

            else
            {
                var errorStrings = "";
                foreach (var error in errors)
                {
                    errorStrings += " " + error.Message + " " + error.Code;
                }
                Assert.Fail($"{code} error code was not found in execution result errors. Found: " + errorStrings);
            }


        }
    }
}