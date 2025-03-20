using HSEBank.Entities.Core;
using HSEBank.UseCases.Repository;

namespace HSEBank.Infrastructure.Repositories.InMemory;

public class InMemoryCategoriesRepository : ICategoriesRepository
{
    private readonly Dictionary<Guid, Category> _categories = new Dictionary<Guid, Category>();

    public void Add(Category entity)
    {
        if (!_categories.TryAdd(entity.Id, entity))
        {
            throw new InvalidOperationException("Category already exists");
        }
    }

    public Category GetById(Guid id)
    {
        if (!_categories.TryGetValue(id, out var category))
        {
            throw new InvalidOperationException("Category not found");
        }
        
        return category;
    }

    public IEnumerable<Category> GetAll()
    {
        return _categories.Values;
    }

    public void Update(Category entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        if (!_categories.ContainsKey(entity.Id))
        {
            throw new InvalidOperationException("Category not found");
        }
        
        _categories[entity.Id] = entity;
    }

    public void Delete(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        if (!_categories.Remove(id))
        {
            throw new InvalidOperationException("Category not found");
        }
    }

    public IEnumerable<Category> GetByName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        return _categories.Values.Where(category => category.Name == name);
    }

    public IEnumerable<Category> GetByType(OperationType type)
    {
        return _categories.Values.Where(category => category.Type == type);
    }
}