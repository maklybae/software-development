using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Repository;

public interface IOperationsRepository : IRepository<Operation>
{
    IEnumerable<Operation> GetByAccount(BankAccount account);
}