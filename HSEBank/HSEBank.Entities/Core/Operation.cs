namespace HSEBank.Entities.Core;

public class Operation : IIdentifiable
{
    public Guid Id { get; private set; }
    
    public OperationType Type { get; private set; }
    
    public BankAccount BankAccount { get; private set; }
    
    public decimal Amount { get; private set; }
    
    public DateTime Date { get; private set; }
    
    public string Description { get; private set; } 
    
    public Category Category { get; private set; }   
    
    public Operation(OperationType type,
        BankAccount bankAccount,
        decimal amount,
        DateTime date,
        string description,
        Category category)
    {
        Type = type;
        BankAccount = bankAccount;
        Amount = amount;
        Date = date;
        Description = description;
        Category = category;
        Id = Guid.NewGuid();
    }
}