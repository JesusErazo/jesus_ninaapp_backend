using Microsoft.AspNetCore.Mvc;
using NinaApp.Core.Exceptions;

namespace NinaApp.API.Middlewares
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class ExceptionHandlingMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public ExceptionHandlingMiddleware(
      RequestDelegate next, 
      ILogger<ExceptionHandlingMiddleware> logger,
      IProblemDetailsService problemDetailsService)
    {
      _next = next;
      _logger = logger;
      _problemDetailsService = problemDetailsService;
     }

    public async Task Invoke(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);

      }catch(Exception ex)
      {
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
      if (ex.InnerException is not null)
      {
        string innExType = ex.InnerException.GetType().ToString();
        string innExMessage = ex.InnerException.Message;
        _logger.LogError($"{innExType}:{innExMessage}");
      }

      string exType = ex.GetType().ToString();
      string exMessage = ex.Message;
      _logger.LogError($"{exType}:{exMessage}");

      int statusCode;
      ProblemDetails problemDetails;

      if(ex is DomainException)
      {
        statusCode = 400;
        problemDetails = new ProblemDetails
        {
          Status = statusCode,
          Title = "Domain Rule Violation",
          Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
          Detail = ex.Message
        };

      }
      else
      {
        statusCode = 500;
        problemDetails = new ProblemDetails
        {
          Status = statusCode,
          Title = "An unexpected error occurred",
          Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
          Detail = "An unexpected error occurred. Please contact the help desk."
        };
      }

      httpContext.Response.StatusCode = statusCode;

      //Use the Framework Service to write the response
      var context = new ProblemDetailsContext
      {
        HttpContext = httpContext,
        ProblemDetails = problemDetails,
      };

      // This tries to write the response using the standard format.
      // If for some reason it fails, we fallback to manual writing.
      if (!await _problemDetailsService.TryWriteAsync(context))
      {
        await httpContext.Response.WriteAsJsonAsync(problemDetails);
      }
    }
  }

  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class ExceptionHandlingMiddlewareExtensions
  {
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
  }
}
