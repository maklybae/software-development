using HSEBank.Entities.Core;
using HSEBank.UseCases.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace HSEBank.Infrastructure.Repositories.Cached;

public class CachedCategoriesRepository(IMemoryCache cache, ICategoriesRepository repository) : ICategoriesRepository
{
    private const string CacheAllKey = "AllCategories";
    private const string CacheKeyPrefix = "Category:";
    
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public void Add(Category entity)
    {
        repository.Add(entity);
        InvalidateCache();
    }

    public Category GetById(Guid id)
    {
        var cacheKey = $"{CacheKeyPrefix}{id}";
        if (cache.TryGetValue(id, out Category? category) && category != null)
        {
            return category;
        }
        
        category = repository.GetById(id);
        cache.Set(id, category, CacheDuration);
        
        return category;
    }

    public IEnumerable<Category> GetAll()
    {
        if (cache.TryGetValue(CacheAllKey, out IEnumerable<Category>? categories) && categories != null)
        {
            return categories;
        }
        
        categories = repository.GetAll();
        var categoryList = categories.ToList();
        cache.Set(CacheAllKey, categoryList, CacheDuration);
        
        return categoryList;
    }

    public void Update(Category entity)
    {
        repository.Update(entity);
        InvalidateCache();
    }

    public void Delete(Guid id)
    {
        var cacheKey = $"{CacheKeyPrefix}{id}";
        repository.Delete(id);
        cache.Remove(cacheKey);
        InvalidateCache();
    }

    public IEnumerable<Category> GetByName(string name)
    {
        // Assume that this type of request is not frequent
        return repository.GetByName(name);
    }

    public IEnumerable<Category> GetByType(OperationType type)
    {
        // Assume that this type of request is not frequent
        return repository.GetByType(type);
    }
    
    private void InvalidateCache()
    {
        cache.Remove(CacheAllKey);
    }
}