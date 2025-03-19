using HSEBank.Entities.Core;
using HSEBank.UseCases.DataSources;
using HSEBank.UseCases.Factories;
using HSEBank.UseCases.Repository;

namespace HSEBank.UseCases.Facades;

public class OperationFacade : IOperationFacade, IOperationsGetter
{
    private readonly IOperationsRepository _operationsRepository;
    private readonly IAccountsRepository _accountRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ICoreEntitiesCreator _factory;
    
    public OperationFacade(IOperationsRepository operationsRepository, ICoreEntitiesCreator factory, IAccountsRepository accountRepository, ICategoriesRepository categoriesRepository)
    {
        _operationsRepository = operationsRepository ?? throw new ArgumentNullException(nameof(operationsRepository));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        _categoriesRepository = categoriesRepository ?? throw new ArgumentNullException(nameof(categoriesRepository));
    }

    public Operation CreatePostOperationByIds(OperationType type, Guid accountId, decimal amount, DateTime date,
        string description,
        Guid? categoryId)
    {
        var account = _accountRepository.GetById(accountId) ??
                      throw new InvalidOperationException($"Account with id {accountId} was not found");
        
        Category? category = null;
        if (categoryId.HasValue)
        {
            category = _categoriesRepository.GetById(categoryId.Value) ??
                       throw new InvalidOperationException($"Category with id {categoryId.Value} was not found");
        }
        
        var operation = _factory.CreateOperation(type, account, amount, date, description, category);
        _operationsRepository.Add(operation);
        return operation;
    }
    
    public Operation GetById(Guid id)
    {
        return _operationsRepository.GetById(id);
    }
    
    public IEnumerable<Operation> GetAll()
    {
        return _operationsRepository.GetAll();
    }
    
    public void UpdateAccountByIds(Guid operationId, Guid accountId)
    {
        var operation = _operationsRepository.GetById(operationId) ??
                        throw new InvalidOperationException($"Operation with id {operationId} was not found");
        var account = _accountRepository.GetById(accountId) ?? 
                      throw new InvalidOperationException($"Account with id {accountId} was not found");
        operation.BankAccount = account;
        _operationsRepository.Update(operation);
    }
    
    public void UpdateAmountById(Guid id, decimal amount)
    {
        var operation = _operationsRepository.GetById(id) ??
                        throw new InvalidOperationException($"Operation with id {id} was not found");
        operation.Amount = amount;
        _operationsRepository.Update(operation);
    }
    
    public void UpdateCategoryToNullById(Guid id)
    {
        var operation = _operationsRepository.GetById(id) ??
                        throw new InvalidOperationException($"Operation with id {id} was not found");
        operation.Category = null;
        _operationsRepository.Update(operation);
    }

    public void UpdateCategoryByIds(Guid operationId, Guid categoryId)
    {
        var operation = _operationsRepository.GetById(operationId) ??
                        throw new InvalidOperationException($"Operation with id {operationId} was not found");
        var category = _categoriesRepository.GetById(categoryId) ??
                       throw new InvalidOperationException($"Category with id {categoryId} was not found");
        operation.Category = category;
        _operationsRepository.Update(operation);
    }
    
    public void UpdateDescriptionById(Guid id, string description)
    {
        var operation = _operationsRepository.GetById(id) ??
                        throw new InvalidOperationException($"Operation with id {id} was not found");
        operation.Description = description;
        _operationsRepository.Update(operation);
    }
    
    public void UpdateDateById(Guid id, DateTime date)
    {
        var operation = _operationsRepository.GetById(id) ??
                        throw new InvalidOperationException($"Operation with id {id} was not found");
        operation.Date = date;
        _operationsRepository.Update(operation);
    }
    
    public void Delete(Guid id)
    {
        _operationsRepository.Delete(id);
    }
}