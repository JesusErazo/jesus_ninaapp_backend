using NinaApp.Core.Common;
using System.Text.Json;

namespace NinaApp.API.Extensions
{
  public static class HttpContextExtensions
  {
    public static void AddHeaderPaginationParameters<T>(this HttpContext httpContext, PagedList<T> data)
    {
      if (httpContext is null) throw new ArgumentNullException(nameof(httpContext));

      var paginationMetadata = new
      {
        data.CurrentPage,
        data.PageSize,
        data.TotalCount,
        data.TotalPages,
        data.HasNext,
        data.HasPrevious
      };

      httpContext.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
    }
  }
}
