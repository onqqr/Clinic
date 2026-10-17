using Microsoft.AspNetCore.Diagnostics;

namespace Clinic.Profile.API.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not InvalidOperationException invalidOperation)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(
            new { message = invalidOperation.Message },
            cancellationToken);

        return true;
    }
}
