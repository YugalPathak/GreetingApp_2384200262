using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Text.Json;

namespace Middleware.GlobalExceptionHandler
{
    /// <summary>
    /// Static class to handle exceptions and generate standardized error responses.
    /// </summary>
    public static class ExceptionHandler
    {
        /// <summary>
        /// Logs the exception and returns a serialized JSON string representing the error response.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="logger">Logger instance to log the error details.</param>
        /// <param name="errorResponse">Out parameter that holds the error response object.</param>
        /// <returns>JSON string representation of the error response.</returns>
        public static string HandleException(Exception ex, Logger logger, out object errorResponse)
        {
            // Logs the error message along with the exception details
            logger.Error(ex, "An error occurred in the application");

            // Creates a structured error response object
            errorResponse = new
            {
                Success = false,
                Message = "An error occurred", // Generic error message
                Error = ex.Message // Detailed exception message
            };

            // Serializes the error response object to a JSON string
            return JsonSerializer.Serialize(errorResponse);
        }

        /// <summary>
        /// Logs the exception and returns an object containing the error response.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        /// <param name="logger">Logger instance to log the error details.</param>
        /// <returns>An object containing error details.</returns>
        public static object CreateErrorResponse(Exception ex, Logger logger)
        {
            // Logs the error message along with the exception details
            logger.Error(ex, "An error occurred in the application");

            // Returns a structured error response object
            return new
            {
                Success = false,
                Message = "An error occurred", // Generic error message
                Error = ex.Message // Detailed exception message
            };
        }
    }
}
