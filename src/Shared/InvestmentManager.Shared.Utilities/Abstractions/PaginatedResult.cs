namespace InvestmentManager.Shared.Utilities.Abstractions;

public class PaginatedResult<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public string? LastId { get; set; }
}