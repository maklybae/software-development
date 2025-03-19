using HSEBank.Entities.Core;

namespace HSEBank.UseCases.DataSources;

public interface IOperationsGetter
{
    IEnumerable<Operation> GetAll();
}