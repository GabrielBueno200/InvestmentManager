using Api.Negotiation.Domain.Entitites;
using Api.Negotiation.Domain.Interfaces;
using InvestmentManager.Shared.Configurations.Settings;
using InvestmentManager.Shared.Utilities.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Api.Negotiation.Infrastructure.Repositories;

public class InvestmentTransactionRepository(IOptions<DatabaseSettings> databaseSettings) 
    : Repository<InvestmentTransaction>(databaseSettings), IInvestmentTransactionRepository
{
    public async Task<PaginatedResult<InvestmentTransaction>> GetByProductIdAsync(string productId, int pageSize, string? lastId = null)
    {
        var filter = Builders<InvestmentTransaction>.Filter.Eq(transaction => transaction.ProductId, productId);
        return await GetFilteredAsync(filter, pageSize, lastId);
    }

    public async Task<PaginatedResult<InvestmentTransaction>> GetUserInvestments(string userId, int pageSize, string? lastId = null)
    {
        var filter = Builders<InvestmentTransaction>.Filter.Eq(transaction => transaction.User.Id, userId);
        return await GetFilteredAsync(filter, pageSize, lastId);
    }
}
