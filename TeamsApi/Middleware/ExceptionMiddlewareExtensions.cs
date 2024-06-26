using System.Net;
using TeamsApi.Exceptions;

namespace TeamsApi.Middleware;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.UseMiddleware<ExceptionMiddleware>();
    }

    public static async Task HandleExceptionAsync(this HttpContext httpContext, Exception exception)
    {
        if (httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        if (exception is null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = exception switch
        {
            BadRequestException => (int)HttpStatusCode.BadRequest,
            InternalServerErrorException => (int)HttpStatusCode.InternalServerError,
            NotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var errorDetails = new ErrorDetails
        {
            ErrorType = exception.GetType().Name,
            Errors = new List<string> { exception.Message }
        };

        await httpContext.Response.WriteAsync(errorDetails.ToString());
    }
}