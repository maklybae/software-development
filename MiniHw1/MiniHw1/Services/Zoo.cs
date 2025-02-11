using MiniHw1.Interfaces;
using MiniHw1.Models.Animals;
using MiniHw1.Models.Things;

namespace MiniHw1.Services;

public class Zoo(IAnimalsRepository animalsRepository, IThingsRepository thingsRepository, IVetClinic vetClinic)
{
    private const int KindnessThreshold = 5;

    public IVetClinic VetClinic { get; set; } = vetClinic;

    public bool TryAddAnimal(Animal animal)
    {
        if (!VetClinic.CheckHealth(animal)) return false;
        animalsRepository.Add(animal);
        return true;
    }
    
    public void AddAnimals(IEnumerable<Animal> animals)
    {
        foreach (var animal in animals)
        {
            TryAddAnimal(animal);
        }
    }
    
    public void AddThing(Thing thing)
    {
        thingsRepository.Add(thing);
    }
    
    public IEnumerable<Animal> Animals => animalsRepository.Get();

    public IEnumerable<Thing> Things => thingsRepository.Get();

    public IEnumerable<Animal> GetPettingAnimals()
    {
        foreach (var animal in Animals)
        {
            if (animal is IKind kindAnimal && kindAnimal.Kindness > KindnessThreshold)
            {
                yield return animal;
            }
        }
    }
    
    public int TotalAnimals => Animals.Count();
    
    public int TotalThings => Things.Count();

    public int GetTotalFood() => Animals.Sum(animal => animal.Food);
}
