namespace sem01;

class Program
{
    static void Main(string[] args)
    {
        var customers = new Queue<Customer>();
        customers.Enqueue(new Customer { Name = "Name1" });
        customers.Enqueue(new Customer { Name = "Name2" });
        customers.Enqueue(new Customer { Name = "Name3" });
        customers.Enqueue(new Customer { Name = "Name4" });
        customers.Enqueue(new Customer { Name = "Name5" });

        var factory = new FactoryAF(customers);

        for (int i = 0; i < 3; i++)
            factory.AddCar();

        Console.WriteLine($"Before:{Environment.NewLine}");
        Console.WriteLine("Cars:");
        Console.WriteLine(string.Join(Environment.NewLine, factory.Cars));
        Console.WriteLine("Customers:");
        Console.WriteLine(string.Join(Environment.NewLine, factory.Customers));
        Console.WriteLine();

        factory.SaleCar();

        Console.WriteLine($"After:{Environment.NewLine}");
        Console.WriteLine("Cars:");
        Console.WriteLine(factory.Cars.Count == 0 ?
            "No cars available" :
            string.Join(Environment.NewLine, factory.Cars));
        Console.WriteLine("Customers:");
        Console.WriteLine(factory.Customers.Count == 0 ?
            "No customers available" :
            string.Join(Environment.NewLine, factory.Customers));
    }
}