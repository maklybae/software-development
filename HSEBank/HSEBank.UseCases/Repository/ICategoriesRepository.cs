using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Repository;

public interface ICategoriesRepository : IRepository<Category>
{
    IEnumerable<Category> GetByName(string name);
    IEnumerable<Category> GetByType(OperationType type);
}