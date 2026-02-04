public class PaginatedList<T> : List<T>
{
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
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}