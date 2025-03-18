using HSEBank.Entities.Core;
using HSEBank.UseCases.Factories;
using HSEBank.UseCases.Repository;

namespace HSEBank.UseCases.Facades;

public class CategoryFacade : ICategoryFacade
{
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly ICoreEntitiesCreator _builder;
    
    public CategoryFacade(ICategoriesRepository categoriesRepository, ICoreEntitiesCreator builder)
    {
        _categoriesRepository = categoriesRepository;
        _builder = builder;
    }
    
    public Category CreatePostCategory(string name, OperationType operationType)
    {
        var category = _builder.CreateCategory(operationType, name);
        _categoriesRepository.Add(category);
        return category;
    }

    public Category GetById(Guid id)
    {
        return _categoriesRepository.GetById(id);
    }
    
    public IEnumerable<Category> GetAll()
    {
        return _categoriesRepository.GetAll();
    }
    
    public void UpdateNameById(Guid id, string name)
    {
        var category = _categoriesRepository.GetById(id) ?? 
                      throw new InvalidOperationException($"Category with id {id} was not found");
        category.Name = name;
        _categoriesRepository.Update(category);
    }

    public void Delete(Guid id)
    {
        _categoriesRepository.Delete(id);
    }
}