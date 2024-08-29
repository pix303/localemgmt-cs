using System.Net;
using System.Text.Json;

namespace Localemgmt.Api.Middleware;


public class ErrorHandlerMiddleware
{
  private readonly RequestDelegate _next;

  public ErrorHandlerMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext httpContext)
  {
    try
    {
      await _next(httpContext);
    }
    catch (System.Exception exp)
    {
      await HandleExeptionAsync(httpContext, exp);
    }
  }


  private static Task HandleExeptionAsync(HttpContext httpContext, Exception exp)
  {
    var httpCode = HttpStatusCode.InternalServerError;
    var result = JsonSerializer.Serialize(new { error = exp.Message });
    httpContext.Response.ContentType = "application/json";
    httpContext.Response.StatusCode = (int)httpCode;

    return httpContext.Response.WriteAsync(result);
  }
}



