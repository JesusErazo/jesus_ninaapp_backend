using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinaApp.Core.Common
{
  public class PagedList<T>
  {
    public List<T> Items { get; private set; }
    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;

    public PagedList(List<T> items, int page, int pageSize, int count)
    {
      Items = items;
      CurrentPage = page;
      PageSize = pageSize;
      TotalCount = count;
      TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
  }
}
