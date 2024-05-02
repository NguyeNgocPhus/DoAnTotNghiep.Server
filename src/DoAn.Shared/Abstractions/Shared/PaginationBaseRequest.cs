namespace DoAn.Shared.Abstractions.Shared;

public class PaginationBaseRequest
{
    public string? Keyword { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderByDesc { get; set; } = Constants.Pagination.DefaultOrderByDesc;
    public int Page { get; set; } = Constants.Pagination.DefaultPage;
    public int Size { get; set; } = Constants.Pagination.DefaultSize;
}