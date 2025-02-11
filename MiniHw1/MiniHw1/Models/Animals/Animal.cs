using MiniHw1.Interfaces;

namespace MiniHw1.Models.Animals;

public abstract class Animal(string name, int food, int health, int number)
    : IAlive, IInventory
{
    public int Food { get; set; } = food;
    public int Health { get; set; } = health;
    public int Number { get; set; } = number;
    public string Name { get; set; } = name;

    public override string ToString()
    {
        return $"{GetType().Name}: Name: {Name}, Food: {Food}, Health: {Health}, Number: {Number}";
    }
}