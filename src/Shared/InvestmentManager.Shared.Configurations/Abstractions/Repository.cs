using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using InvestmentManager.Shared.Utilities.Helpers;
using InvestmentManager.Shared.Configurations.Settings;

namespace InvestmentManager.Shared.Utilities.Abstractions;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IMongoCollection<TEntity> _collection;

    private readonly DatabaseSettings _databaseSettings;

    public Repository(IOptions<DatabaseSettings> databaseSettings)
    {
        _databaseSettings = databaseSettings.Value;
        var database = new MongoClient(_databaseSettings.ConnectionString).GetDatabase(_databaseSettings.DatabaseName);
        var collectionName = CollectionHelpers.GetCollectionName<TEntity>();
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    public async Task<TEntity> GetByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
    }

    public async Task<PaginatedResult<TEntity>> GetAllAsync(int pageSize, string? lastId = null)
    {
        return await GetFilteredAsync(Builders<TEntity>.Filter.Empty, pageSize, lastId);
    }

    public async Task<PaginatedResult<TEntity>> GetFilteredAsync(FilterDefinition<TEntity> filter, int pageSize, string? lastId = null)
    {
        if (!string.IsNullOrEmpty(lastId))
        {
            filter = Builders<TEntity>.Filter.Gt(product => product.Id, lastId);
        }

        var items = await _collection.Find(filter)
            .Sort(Builders<TEntity>.Sort.Ascending(product => product.Id))
            .Limit(pageSize)
            .ToListAsync();

        return new PaginatedResult<TEntity>
        {
            Items = items,
            TotalCount = items.Count,
            PageSize = pageSize,
            LastId = items.LastOrDefault()?.Id
        };
    }

    public async Task<IEnumerable<TEntity>> FilterByAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.Id = ObjectId.GenerateNewId().ToString();
        await _collection.InsertOneAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(TEntity entity)
    {
        entity.UpdatedAt = DateTime.Now;
        var filter = Builders<TEntity>.Filter.Eq("_id", entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteByIdAsync(string id)
    {
        var filter = Builders<TEntity>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }

    public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
    {
        await _collection.DeleteManyAsync(filterExpression);
    }
}