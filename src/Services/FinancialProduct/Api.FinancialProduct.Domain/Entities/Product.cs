using Api.FinancialProduct.Domain.Constants;
using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Helpers;

namespace Api.FinancialProduct.Domain.Entities;

[Collection("products")]
public class Product : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? MaturityDate { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public int Type { get; set; }
    public IList<decimal> PriceHistory { get; set; }
}
