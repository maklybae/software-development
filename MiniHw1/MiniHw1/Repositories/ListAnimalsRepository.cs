using MiniHw1.Interfaces;
using MiniHw1.Models;
using MiniHw1.Models.Animals;

namespace MiniHw1.Repositories;

public class ListAnimalsRepository : IAnimalsRepository
{
    private List<Animal> _data = [];
    
    public void Add(Animal animal)
    {
        _data.Add(animal);
    }

    public void Remove(Animal animal)
    {
        _data.Remove(animal);
    }

    public IEnumerable<Animal> Get()
    {
        return _data;
    }
}