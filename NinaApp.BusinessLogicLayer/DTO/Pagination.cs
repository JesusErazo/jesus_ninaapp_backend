namespace NinaApp.Core.DTO
{
  public record Pagination(int Page = 1, int PageSize = 10)
  {
    private const int MAXIMUM_PAGE_SIZE = 30;
    public int Page { get; init; } = Math.Max(1, Page);
    public int PageSize { get; init; } = Math.Clamp(PageSize, 1, MAXIMUM_PAGE_SIZE);
  }
}
