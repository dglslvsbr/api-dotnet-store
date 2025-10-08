using System.Diagnostics;

namespace StoreAPI.Middlewares;

public class ResponseTimeMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var responseTime = Stopwatch.StartNew();

        await _next(context);

        responseTime.Stop();

        var time = responseTime.ElapsedMilliseconds;

        if (!context.Response.HasStarted)
            context.Response.Headers["X-Response-Time-ms"] = time.ToString();

        Console.WriteLine($"[{context.Request.Method}] {context.Request.Path} - {time}ms");
    }
}