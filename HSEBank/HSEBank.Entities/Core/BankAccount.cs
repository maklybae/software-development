using HSEBank.Entities.Visitors;

namespace HSEBank.Entities.Core;

public class BankAccount : IIdentifiable, ICoreEntityVisitable
{
    private string _name;
    private decimal _balance;
    
    public Guid Id { get; private set; }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }

            _name = value;
        }
    }

    public decimal Balance
    {
        get => _balance;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Balance cannot be negative.");
            }
            
            _balance = value;
        }
    }
    
    public BankAccount(string name, decimal balance)
    {
        Name = name;
        Balance = balance;
        Id = Guid.NewGuid();
    }
    
    public void IncreaseBalance(decimal amount)
    {
        // Validation in the setter
        Balance += amount;
    }

    public void DecreaseBalance(decimal amount)
    {
        // Validation in the setter
        Balance -= amount;
    }

    public void Accept(ICoreEntityVisitor visitor)
    {
        visitor.Visit(this);
    }
}