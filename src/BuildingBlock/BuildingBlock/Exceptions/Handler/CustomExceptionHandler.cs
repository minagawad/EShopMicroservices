using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlock.Exceptions.Handler
{
    public class CustomExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<CustomExceptionHandler> logger;
        public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
        {
            this.logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionMessage = exception.Message;
            logger.LogError(
                "Error Message: {exceptionMessage}, Time of occurrence {time}",
                exceptionMessage, DateTime.UtcNow);

            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),
                ValidationException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                BadRequestException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: httpContext.Response.StatusCode = StatusCodes.Status400BadRequest
                ),
                NotFoundException =>
                (
                    Detail: exception.Message,
                    Title: exception.GetType().Name,
                    StatusCode: httpContext.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                _ => (
                    Detail: "An unexpected error occurred.",
                    Title: "InternalServerError",
                    StatusCode: httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError
                )
            };
            var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode
            };
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            if (exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                problemDetails.Extensions["errors"] = errors;
            }
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            // Return false to continue with the default behavior
            // - or - return true to signal that this exception is handled
            return true;
        }
    }
}
