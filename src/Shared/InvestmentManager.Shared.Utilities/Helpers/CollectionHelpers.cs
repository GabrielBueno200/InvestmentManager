using System;

namespace InvestmentManager.Shared.Utilities.Helpers;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class CollectionAttribute : Attribute
{
    public string CollectionName { get; private set; }
    public CollectionAttribute(string collectionName) => CollectionName = collectionName;
}

public static class CollectionHelpers
{
    public static string GetCollectionName<TEntity>() where TEntity : class
    {
        var entityType = typeof(TEntity);
        var collectionAttribute = Attribute.GetCustomAttribute(entityType, typeof(CollectionAttribute));

        if (collectionAttribute is not CollectionAttribute attribute)
        {
            return string.Empty;
        }

        return attribute?.CollectionName ?? entityType.Name.ToLowerInvariant();
    }
}
