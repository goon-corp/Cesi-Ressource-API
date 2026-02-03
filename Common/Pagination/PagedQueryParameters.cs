namespace Ressource_API.Common.Pagination;

public class PagedQueryParameters
{
    private const int MaxSize = 50;
    private int _size = 10;
    private int _page = 1;

    public int page
    {
        get => _page;
        set => _page = (value < 1) ? 1 : value;
    }

    // Ensure 'size' is within valid bounds (1 to MaxSize)
    public int size
    {
        get => _size;
        set => _size = (value < 1) ? 1 : (value > MaxSize ? MaxSize : value);
    }
}