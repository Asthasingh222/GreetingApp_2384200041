using System;
using Microsoft.Extensions.Logging;

namespace Middleware
{
    public static class ExceptionHandler
    {
        public static object CreateErrorResponse(Exception ex, ILogger logger)
        {
            logger.LogError(ex, "Exception occurred.");

            return new
            {
                success = false,
                error = "An unexpected error occurred.",
                message = ex.Message
            };
        }
    }
}
