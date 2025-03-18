using HSEBank.Entities.Core;
using HSEBank.UseCases.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace HSEBank.Infrastructure.Repositories.Cached;

public class CachedAccountsRepository(IAccountsRepository repository, IMemoryCache cache) : IAccountsRepository
{
    private const string CacheAllKey = "AllAccounts";
    private const string CacheKeyPrefix = "Account:";
    
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    
    public void Add(BankAccount entity)
    {
        repository.Add(entity);
        InvalidateCache();
    }

    public BankAccount GetById(Guid id)
    {
        var cacheKey = $"{CacheKeyPrefix}{id}";
        if (cache.TryGetValue(id, out BankAccount? account) && account != null)
        {
            return account;
        }

        account = repository.GetById(id);
        cache.Set(id, account, CacheDuration);

        return account;
    }

    public IEnumerable<BankAccount> GetAll()
    {
        if (cache.TryGetValue(CacheAllKey, out IEnumerable<BankAccount>? accounts) && accounts != null)
        {
            return accounts;
        }
        
        accounts = repository.GetAll();
        var bankAccounts = accounts.ToList();
        cache.Set(CacheAllKey, bankAccounts, CacheDuration);
        
        return bankAccounts;
    }

    public void Update(BankAccount entity)
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

    public IEnumerable<BankAccount> GetByName(string name)
    {
        // Assume that this type of request is not frequent
        return repository.GetByName(name);
    }
    
    private void InvalidateCache()
    {
        cache.Remove(CacheAllKey);
    }
}