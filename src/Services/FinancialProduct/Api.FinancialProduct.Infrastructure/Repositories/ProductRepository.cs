using MongoDB.Driver;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using InvestmentManager.Shared.Configurations.Settings;
using InvestmentManager.Shared.Utilities.Abstractions;
using Microsoft.Extensions.Options;

namespace Api.FinancialProduct.Infrastructure.Repositories;

public class ProductRepository(IOptions<DatabaseSettings> databaseSettings) 
    : Repository<Product>(databaseSettings), IProductRepository
{
    public async Task<PaginatedResult<Product>> GetAvailableProductsAsync(int pageSize, string? lastId = null)
    {
        var filter = GetAvailableProductsFilter();
        return await GetFilteredAsync(filter, pageSize, lastId);
    }

    private FilterDefinition<Product> GetAvailableProductsFilter()
    {
        var oneOrMoreAmountFilter = Builders<Product>.Filter.Gt(product => product.Amount, 0);
        var maturityDateNotPassedFilter = Builders<Product>.Filter.Gte(product => product.MaturityDate, DateTime.Today);

        return Builders<Product>.Filter.And(oneOrMoreAmountFilter, maturityDateNotPassedFilter);
    }
};
