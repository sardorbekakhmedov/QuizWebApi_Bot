namespace QuizWebApi_Bot.Middleware;

public class QuizWebApiErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<QuizWebApiErrorMiddleware> _logger;

    public QuizWebApiErrorMiddleware(RequestDelegate next,
        ILogger<QuizWebApiErrorMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {

        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal QUIZ_WEB_API server error!");
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Error = e.Message,
            });

        }
    }
}


public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomQuizWebApiErrorMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<QuizWebApiErrorMiddleware>();
    }
}