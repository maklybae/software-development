namespace sem01;

public class Car
{
    private static int _number = 1; 
    private static readonly Random Random = new();

    public int Number { get; } = _number++;

    public Engine Engine { get; } = new() { Size = Random.Next(1, 10) };

    public override string ToString()
    {
        return $"S/N: {Number}; Pedal size: {Engine.Size}";
    }
}