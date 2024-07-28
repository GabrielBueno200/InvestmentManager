
namespace InvestmentManager.Shared.Utilities.Contracts.Dtos;

public class ProductResponseDto
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? MaturityDate { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public int Type { get; set; }
    public IList<decimal> PriceHistory { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
