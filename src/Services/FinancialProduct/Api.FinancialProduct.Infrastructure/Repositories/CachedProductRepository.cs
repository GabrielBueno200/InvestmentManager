using MongoDB.Driver;
using System.Linq.Expressions;
using Api.FinancialProduct.Domain.Entities;
using Api.FinancialProduct.Domain.Interfaces.Repositories;
using InvestmentManager.Shared.Utilities.Abstractions;
using StackExchange.Redis;
using System.Text.Json;

namespace Api.FinancialProduct.Infrastructure.Repositories;

public class CachedProductRepository(IProductRepository decorated, IConnectionMultiplexer muxer) : IProductRepository
{
    private readonly IProductRepository _decorated = decorated;
    private readonly IDatabase _database = muxer.GetDatabase();

    public async Task<PaginatedResult<Product>> GetAvailableProductsAsync(int pageSize, string? lastId = null)
    {
        var cacheKey = $"available-products-{pageSize}{lastId ?? string.Empty}";
        var cachedData = await _database.StringGetAsync(cacheKey);

        if (cachedData.HasValue)
        {
            var paginatedResult = JsonSerializer.Deserialize<PaginatedResult<Product>>(cachedData!);
            return paginatedResult!;
        }

        var result = await _decorated.GetAvailableProductsAsync(pageSize, lastId);

        await _database.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(1));

        return result;
    }

    public Task<Product> AddAsync(Product document) => _decorated.AddAsync(document);

    public Task DeleteByIdAsync(string id) => _decorated.DeleteByIdAsync(id);

    public Task DeleteManyAsync(Expression<Func<Product, bool>> filterExpression) 
        => _decorated.DeleteManyAsync(filterExpression);

    public Task<IEnumerable<Product>> FilterByAsync(Expression<Func<Product, bool>> filterExpression)
        => _decorated.FilterByAsync(filterExpression);

    public Task<IEnumerable<Product>> GetAllAsync() => _decorated.GetAllAsync();

    public Task<PaginatedResult<Product>> GetAllAsync(int pageSize, string? lastId = null)
        => _decorated.GetAllAsync(pageSize, lastId);

    public Task<Product> GetByIdAsync(string id) => _decorated.GetByIdAsync(id);

    public Task<PaginatedResult<Product>> GetFilteredAsync(FilterDefinition<Product> filter, int pageSize, string? lastId = null)
        => _decorated.GetFilteredAsync(filter, pageSize, lastId);

    public Task UpdateAsync(Product document) => _decorated.UpdateAsync(document);
}
