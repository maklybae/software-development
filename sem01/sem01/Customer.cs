namespace sem01;

public class Customer
{
    public required string Name { get; set; }

    public Car? Car { get; set; }

    public override string ToString()
    {
        return Car == null ? $"Имя: {Name}, Машины нет" : $"Имя: {Name}, Номер машины: {Car?.Number ?? -1}";
    }
}