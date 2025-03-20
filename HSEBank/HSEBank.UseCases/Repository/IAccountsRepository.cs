using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Repository;

public interface IAccountsRepository : IRepository<BankAccount>
{
    IEnumerable<BankAccount> GetByName(string name);
}