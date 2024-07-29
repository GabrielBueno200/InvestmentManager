using System.Linq.Expressions;
using MongoDB.Driver;

namespace InvestmentManager.Shared.Utilities.Abstractions;

public interface IRepository<TEntity> where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<PaginatedResult<TEntity>> GetAllAsync(int pageSize, string? lastId = null);
    Task<PaginatedResult<TEntity>> GetFilteredAsync(FilterDefinition<TEntity> filter, int pageSize, string? lastId = null);
    Task<IEnumerable<TEntity>> GetFilteredAsync(FilterDefinition<TEntity> filter);
    Task<TEntity> GetByIdAsync(string id);
    Task<IEnumerable<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task<TEntity> AddAsync(TEntity document);
    Task UpdateAsync(TEntity document);
    Task DeleteByIdAsync(string id);
    Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression);
}