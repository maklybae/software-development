namespace HSEBank.Entities.Core;

public class BankAccount : IIdentifiable
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public decimal Balance { get; private set; }
}