using InvestmentManager.Shared.Utilities.Abstractions;

namespace InvestmentManager.Shared.Utilities.Contracts.Dtos;

public class InvestmentTransactionResponseDto
{
    public string Id { get; set ;}
    public string ProductId { get; set; }
    public SummarizedUser User { get; set; }
    public int Amount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
