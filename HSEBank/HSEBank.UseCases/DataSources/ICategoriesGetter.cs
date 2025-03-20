using HSEBank.Entities.Core;

namespace HSEBank.UseCases.DataSources;

public interface ICategoriesGetter
{
    IEnumerable<Category> GetAll();
}