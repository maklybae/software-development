using HSEBank.Entities.Core;
using HSEBank.UseCases.DataSources;

namespace HSEBank.UseCases.Facades;

public interface IBankAccountFacade : IBankAccountsGetter
{
    BankAccount CreatePostAccount(string name, decimal balance);
    BankAccount GetById(Guid id);
    BankAccount GetUniqueByName(string name);
    IEnumerable<BankAccount> GetAccounts();
    void UpdateNameById(Guid id, string name);
    void IncreaseBalanceById(Guid id, decimal amount);
    void DecreaseBalanceById(Guid id, decimal amount);
    void Delete(Guid id);
}