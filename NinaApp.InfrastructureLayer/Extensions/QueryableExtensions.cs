using Microsoft.EntityFrameworkCore;
using NinaApp.Core.Common;

namespace NinaApp.Infrastructure.Extensions
{
  public static class QueryableExtensions
  {
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
      this IQueryable<T> source,
      int page,
      int pageSize)
    {
      var count = await source.CountAsync();

      var items = await source
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

      return new PagedList<T>(items,page,pageSize, count);
    }
  }
}
