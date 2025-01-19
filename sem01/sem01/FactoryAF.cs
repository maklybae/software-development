namespace sem01;

public interface ICarsSaleable
{
    void SaleCar();
}

public interface ICarsAddable
{
    void AddCar();
}

public class FactoryAF : ICarsSaleable, ICarsAddable
{
    public List<Car> Cars { get; } = new List<Car>();

    public Queue<Customer> Customers { get; init; }

    public FactoryAF(Queue<Customer> customers)
    {
        Customers = customers;
    }

    public void SaleCar()
    {
        while (Customers.Count > 0 && Cars.Count > 0)
        {
            var customer = Customers.Dequeue();
            customer.Car = Cars.Last();
        
            Cars.RemoveAt(Cars.Count - 1);
        }
        
        Cars.Clear();
    }

    public void AddCar()
    {
        var car = new Car();
        Cars.Add(car);
    }
}