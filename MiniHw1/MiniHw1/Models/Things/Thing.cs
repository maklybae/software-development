using MiniHw1.Interfaces;

namespace MiniHw1.Models.Things;

public class Thing(int number) : IInventory
{
    public int Number { get; set; } = number;

    public override string ToString()
    {
        return $"{GetType().Name}: Number: {Number}";
    }
}