using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Repository;

/// <summary>
/// Basic CRUD-operation interface
/// </summary>
/// <typeparam name="T">Type of domain entity</typeparam>
public interface IRepository<T> where T : IIdentifiable
{
    // Create
    void Add(T entity);
    
    // Read
    T GetById(Guid id);
    IEnumerable<T> GetAll();
    
    // Update
    void Update(T entity);
    
    // Delete
    void Delete(Guid id);
}