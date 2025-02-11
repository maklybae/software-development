using MiniHw1.Interfaces;

namespace MiniHw1.Models.Animals;

public abstract class Herbo(string name, int food, int health, int number) :
    Animal(name, food, health, number), IKind
{
    private const int DefaultKindness = 0; // unkind by default
    
    public int Kindness { get; set; } = 0;

    public override string ToString()
    {
        return $"{GetType().Name}: Name: {Name}, Food: {Food}, Health: {Health}, Number: {Number}, Kindness: {Kindness}";
    }
}