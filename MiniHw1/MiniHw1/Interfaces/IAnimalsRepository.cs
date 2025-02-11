using MiniHw1.Models.Animals;

namespace MiniHw1.Interfaces;

public interface IAnimalsRepository
{
    void Add(Animal animal);
    void Remove(Animal animal);
    IEnumerable<Animal> Get();
}