using InvestmentManager.Shared.Utilities.Abstractions;
using InvestmentManager.Shared.Utilities.Helpers;

namespace Api.Negotiation.Domain.Entitites;

[Collection("investmentTransaction")]
public class InvestmentTransaction : BaseEntity
{
    public string ProductId { get; set; }
    public SummarizedUser User { get; set; }
    public int Amount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Type { get; set; }
}
