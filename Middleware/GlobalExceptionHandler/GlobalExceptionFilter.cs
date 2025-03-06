using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NLog;
using System;

namespace Middleware.GlobalExceptionHandler
{
    /// <summary>
    /// Global exception filter to handle unhandled exceptions in the application.
    /// Logs the exceptions and returns a generic error response.
    /// </summary>
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        // Logger instance to log exceptions
        private readonly Logger<GlobalExceptionFilter> _logger;
        private Logger logger; // Unused logger variable, should be removed if not needed

        /// <summary>
        /// Constructor to initialize the logger.
        /// </summary>
        /// <param name="logger">Logger instance for logging exceptions</param>
        public GlobalExceptionFilter(Logger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method is triggered when an unhandled exception occurs in the application.
        /// It logs the error and sets the HTTP response accordingly.
        /// </summary>
        /// <param name="context">The exception context</param>
        public override void OnException(ExceptionContext context)
        {
            // Creates a standardized error response using a custom exception handler
            var errorResponse = ExceptionHandler.CreateErrorResponse(context.Exception, logger);

            // Sets the response with a 500 status code (Internal Server Error)
            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500
            };

            // Marks the exception as handled to prevent further propagation
            context.ExceptionHandled = true;
        }
    }
}
