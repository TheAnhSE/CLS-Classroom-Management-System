namespace CLS.Common.Pagination;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public int CurrentPage { get; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        Items = items;
        TotalCount = count;
        CurrentPage = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }
}
