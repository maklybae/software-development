using HSEBank.Entities.Core;
using HSEBank.UseCases.Repository;
using HSEBank.UseCases.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace HSEBank.Infrastructure.Repositories.Cached;

public class CachedOperationsRepository(IOperationsRepository repository, IMemoryCache cache) : IOperationsRepository
{
    private const string CacheAllKey = "AllOperations";
    private const string CacheKeyPrefix = "Operation:";

    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public void Add(Operation entity)
    {
        repository.Add(entity);
        InvalidateCache();
    }

    public Operation GetById(Guid id)
    {
        var cacheKey = $"{CacheKeyPrefix}{id}";
        if (cache.TryGetValue(id, out Operation? operation) && operation != null)
        {
            return operation;
        }
        
        operation = repository.GetById(id);
        cache.Set(id, operation, CacheDuration);
        
        return operation;
    }

    public IEnumerable<Operation> GetAll()
    {
        if (cache.TryGetValue(CacheAllKey, out IEnumerable<Operation>? operations) && operations != null)
        {
            return operations;
        }
        
        operations = repository.GetAll();
        var operationList = operations.ToList();
        cache.Set(CacheAllKey, operationList, CacheDuration);
        
        return operationList;
    }

    public void Update(Operation entity)
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

    public IEnumerable<Operation> GetByAccount(BankAccount account)
    {
        // Assume that this type of request is not frequent
        return repository.GetByAccount(account);
    }
    
    private void InvalidateCache()
    {
        cache.Remove("AllOperations");
    }
}