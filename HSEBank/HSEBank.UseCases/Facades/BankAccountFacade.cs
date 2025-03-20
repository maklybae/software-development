using HSEBank.Entities.Core;
using HSEBank.UseCases.DataSources;
using HSEBank.UseCases.Factories;
using HSEBank.UseCases.Repository;

namespace HSEBank.UseCases.Facades;

public class BankAccountFacade : IBankAccountFacade
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly ICoreEntitiesCreator _factory; 

    public BankAccountFacade(IAccountsRepository accountsRepository, ICoreEntitiesCreator factory)
    {
        _accountsRepository = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }
    
    public BankAccount CreatePost(string name, decimal balance)
    {
        var account = _factory.CreateBankAccount(name, balance);
        _accountsRepository.Add(account);
        return account;
    }

    public BankAccount CreatePost(Guid id, string name, decimal balance)
    {
        var account = _factory.CreateBankAccount(id, name, balance);
        _accountsRepository.Add(account);
        return account;
    }

    public BankAccount GetById(Guid id)
    {
        return _accountsRepository.GetById(id);
    }
    
    public BankAccount GetUniqueByName(string name)
    {
        var account = _accountsRepository.GetByName(name).FirstOrDefault();
        return account ?? throw new InvalidOperationException($"Account with name {name} was not found");
    }

    public IEnumerable<BankAccount> GetAll()
    {
        return _accountsRepository.GetAll();
    }
    
    public void UpdateNameById(Guid id, string name)
    {
        var account = _accountsRepository.GetById(id);
        account.Name = name;
        _accountsRepository.Update(account);
    }
    
    public void IncreaseBalanceById(Guid id, decimal amount)
    {
        var account = _accountsRepository.GetById(id) ?? 
                      throw new InvalidOperationException($"Account with name {id} was not found");
        account.IncreaseBalance(amount);
        _accountsRepository.Update(account);
    }

    public void DecreaseBalanceById(Guid id, decimal amount)
    {
        var account = _accountsRepository.GetById(id) ?? 
                      throw new InvalidOperationException($"Account with name {id} was not found");
        account.DecreaseBalance(amount);
        _accountsRepository.Update(account);
    }
    
    public void Delete(Guid id)
    {
        _accountsRepository.Delete(id);
    }
}