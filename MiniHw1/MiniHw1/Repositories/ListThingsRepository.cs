using MiniHw1.Interfaces;
using MiniHw1.Models;
using MiniHw1.Models.Things;

namespace MiniHw1.Repositories;

public class ListThingsRepository : IThingsRepository
{
    private List<Thing> _data = [];
    
    public void Add(Thing thing)
    {
        _data.Add(thing);
    }

    public void Remove(Thing thing)
    {
        _data.Remove(thing);
    }

    public IEnumerable<Thing> Get()
    {
        return _data;
    }
}