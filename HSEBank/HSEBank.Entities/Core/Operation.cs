using HSEBank.Entities.Visitors;

namespace HSEBank.Entities.Core;

public class Operation : ICoreEntity
{
    private BankAccount _bankAccount;
    private decimal _amount;
    private DateTime _date;
    private Category? _category;
    
    public Guid Id { get; private set; }
    
    // Set once in ctor. Maintain the invariant. Check Category.
    public OperationType Type { get; private init; }

    public BankAccount BankAccount
    {
        get => _bankAccount;
        set => _bankAccount = value ?? throw new ArgumentNullException(nameof(value), "Bank account cannot be null.");
    }

    public decimal Amount
    {
        get => _amount;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Amount cannot be negative or zero", nameof(value));
            }

            _amount = value;
        }
    }

    public DateTime Date
    {
        get => _date;
        set
        {
            if (value == default)
            {
                throw new ArgumentException("Date cannot be default", nameof(value));
            }

            _date = value;
        }
    }
    
    public string Description { get; set; }

    public Category? Category
    {
        get => _category;
        set
        {
            if (value != null && value.Type != Type)
            {
                throw new ArgumentException("Category type must match operation type.");
            }
            
            _category = value;
        }
    }

    public Operation(Guid id,
        OperationType type,
        BankAccount bankAccount,
        decimal amount,
        DateTime date,
        string description = "",
        Category? category = null)
    {
        Id = id;
        Type = type;
        BankAccount = bankAccount;
        Amount = amount;
        Date = date;
        Description = description;
        Category = category;
    }
    
    public Operation(OperationType type,
        BankAccount bankAccount,
        decimal amount,
        DateTime date,
        string description = "",
        Category? category = null)
    {
        Type = type;
        BankAccount = bankAccount;
        Amount = amount;
        Date = date;
        Description = description;
        Category = category;
        Id = Guid.NewGuid();
    }

    public void Accept(ICoreEntityVisitor visitor)
    {
        visitor.Visit(this);
    }
}