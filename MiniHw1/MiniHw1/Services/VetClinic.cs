using MiniHw1.Interfaces;

namespace MiniHw1.Services;

public class VetClinic : IVetClinic
{
    private const int MaxHealth = 10;
    private const int HealthyThreshold = 5;
    
    public bool CheckHealth(IAlive creature)
    {
        return creature.Health > HealthyThreshold && creature.Health <= MaxHealth;
    }
}