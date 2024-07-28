using Api.Negotiation.Domain.Entitites;
using InvestmentManager.Shared.Utilities.Abstractions;

namespace Api.Negotiation.Domain.Interfaces;

public interface IInvestmentTransactionRepository : IRepository<InvestmentTransaction>
{
    Task<PaginatedResult<InvestmentTransaction>> GetByProductIdAsync(string productId, int pageSize, string? lastId = null);
}
