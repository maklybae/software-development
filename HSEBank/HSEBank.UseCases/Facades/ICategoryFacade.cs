using HSEBank.Entities.Core;
using HSEBank.UseCases.DataSources;

namespace HSEBank.UseCases.Facades;

public interface ICategoryFacade : ICategoriesGetter
{
    Category CreatePost(string name, OperationType operationType);
    Category CreatePost(Guid id, string name, OperationType operationType);
    Category GetById(Guid id);
    void UpdateNameById(Guid id, string name);
    void Delete(Guid id);
}