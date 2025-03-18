using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Facades;

public interface IBankAccountFacade
{
    BankAccount CreatePostAccount(string name, decimal balance);
    BankAccount GetById(Guid id);
    BankAccount GetUniqueByName(string name);
    IEnumerable<BankAccount> GetAll();
    IEnumerable<BankAccount> GetAccounts();
    void UpdateNameById(Guid id, string name);
    void IncreaseBalanceById(Guid id, decimal amount);
    void DecreaseBalanceById(Guid id, decimal amount);
    void Delete(Guid id);
}