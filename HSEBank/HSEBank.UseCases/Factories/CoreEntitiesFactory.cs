using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Factories;

public class CoreEntitiesFactory : ICoreEntitiesCreator
{
    public BankAccount CreateBankAccount(string name, decimal balance = decimal.Zero)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }
    
        if (balance < 0)
        {
            throw new ArgumentException("Balance cannot be negative", nameof(balance));
        }
    
        return new BankAccount(name, balance);
    }

    public Operation CreateOperation(OperationType type, BankAccount account, decimal amount, DateTime date, string description,
        Category? category)
    {
        ArgumentNullException.ThrowIfNull(account);

        if (amount <= 0)
        {
            throw new ArgumentException("Amount cannot be negative or zero", nameof(amount));
        }
    
        if (date == default)
        {
            throw new ArgumentException("Date cannot be default", nameof(date));
        }
    
        return new Operation(type, account, amount, date, description, category);
    }

    public Category CreateCategory(OperationType type, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }
    
        return new Category(type, name);
    }
}