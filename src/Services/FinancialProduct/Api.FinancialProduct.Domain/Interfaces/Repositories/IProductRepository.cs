using Api.FinancialProduct.Domain.Entities;
using InvestmentManager.Shared.Utilities.Abstractions;

namespace Api.FinancialProduct.Domain.Interfaces.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<PaginatedResult<Product>> GetAvailableProductsAsync(int pageSize, string? lastId = null);
}
