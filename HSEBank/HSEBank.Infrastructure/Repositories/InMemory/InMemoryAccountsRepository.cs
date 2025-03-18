using HSEBank.Entities.Core;
using HSEBank.UseCases.Repository;

namespace HSEBank.Infrastructure.Repositories.InMemory;

public class InMemoryAccountsRepository : IAccountsRepository
{
    private readonly Dictionary<Guid, BankAccount> _accounts = new();
    
    public void Add(BankAccount entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
    
        if (!_accounts.TryAdd(entity.Id, entity))
        {
            throw new InvalidOperationException("Account already exists");
        }
    }

    public BankAccount GetById(Guid id)
    {
        if (!_accounts.TryGetValue(id, out var account))
        {
            throw new InvalidOperationException("Account not found");
        }

        return account;
    }

    public IEnumerable<BankAccount> GetAll()
    {
        return _accounts.Values;
    }

    public void Update(BankAccount entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (!_accounts.ContainsKey(entity.Id))
        {
            throw new InvalidOperationException("Account not found");
        }

        _accounts[entity.Id] = entity;
    }

    public void Delete(Guid id)
    {
        if (!_accounts.Remove(id))
        {
            throw new InvalidOperationException("Account not found");
        }
    }

    public IEnumerable<BankAccount> GetByName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        return _accounts.Values.Where(account => account.Name == name);
    }
}