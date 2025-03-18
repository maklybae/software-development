using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Facades;

public interface ICategoryFacade
{
    Category CreatePostCategory(string name, OperationType operationType);
    Category GetById(Guid id);
    IEnumerable<Category> GetAll();
    void UpdateNameById(Guid id, string name);
    void Delete(Guid id);
}