namespace Ressource_API.Common.Pagination;

public class PaginatedList<T>
{
    public List<T> Items { get; set; }
    public int PageIndex  { get; }
    public int PageSize   { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public PaginatedList(
        List<T> items,
        int pageIndex,
        int pageSize,
        int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}