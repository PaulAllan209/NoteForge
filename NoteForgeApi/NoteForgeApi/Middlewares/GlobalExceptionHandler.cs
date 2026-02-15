using Microsoft.AspNetCore.Diagnostics;
using NoteForgeApi.Models;

namespace NoteForgeApi.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                _ => StatusCodes.Status500InternalServerError
            };

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse
            {
                StatusCode = statusCode,
                Message = exception.Message,
                TraceId = httpContext.TraceIdentifier
            }, cancellationToken);

            return true;
        }
    }
}
