using HSEBank.Entities.Core;

namespace HSEBank.UseCases.DataSources;

public interface IBankAccountsGetter
{
    IEnumerable<BankAccount> GetAll();
}