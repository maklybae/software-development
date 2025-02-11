using MiniHw1.Models.Things;

namespace MiniHw1.Interfaces;

public interface IThingsRepository
{
    void Add(Thing thing);
    void Remove(Thing thing);
    IEnumerable<Thing> Get();
}