using HSEBank.Entities.Core;
using HSEBank.UseCases.Repository;

namespace HSEBank.Infrastructure.Repositories.InMemory;

public class InMemoryOperationsRepository : IOperationsRepository
{
    private readonly Dictionary<Guid, Operation> _operations = new();
    
    public void Add(Operation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (!_operations.TryAdd(entity.Id, entity))
        {
            throw new InvalidOperationException("Operation already exists");
        }
    }

    public Operation GetById(Guid id)
    {
        if (!_operations.TryGetValue(id, out var operation))
        {
            throw new InvalidOperationException("Operation not found");
        }

        return operation;
    }

    public IEnumerable<Operation> GetAll()
    {
        return _operations.Values;
    }

    public void Update(Operation entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (!_operations.ContainsKey(entity.Id))
        {
            throw new InvalidOperationException("Operation not found");
        }

        _operations[entity.Id] = entity;
    }

    public void Delete(Guid id)
    {
        if (!_operations.Remove(id))
        {
            throw new InvalidOperationException("Operation not found");
        }
    }

    public IEnumerable<Operation> GetByAccount(BankAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);
        
        return _operations.Values.Where(operation => operation.BankAccount.Id == account.Id);
    }
}