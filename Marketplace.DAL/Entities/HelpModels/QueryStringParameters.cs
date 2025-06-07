namespace Marketplace.DAL.Entities.HelpModels;

public abstract class QueryStringParameters
{
    private const int MaxPageSize = 100;
    public int PageNumber { get; set; } = 1;
    
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    
    public string? OrderBy { get; set; }
}