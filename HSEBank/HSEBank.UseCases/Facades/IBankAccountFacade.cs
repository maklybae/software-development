using HSEBank.Entities.Core;
using HSEBank.UseCases.DataSources;

namespace HSEBank.UseCases.Facades;

public interface IBankAccountFacade : IBankAccountsGetter
{
    BankAccount CreatePost(string name, decimal balance);
    BankAccount CreatePost(Guid id, string name, decimal balance);
    BankAccount GetById(Guid id);
    BankAccount GetUniqueByName(string name);
    void UpdateNameById(Guid id, string name);
    void IncreaseBalanceById(Guid id, decimal amount);
    void DecreaseBalanceById(Guid id, decimal amount);
    void Delete(Guid id);
}