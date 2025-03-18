namespace HSEBank.Entities.Core;

public class Category : IIdentifiable
{
    public Guid Id { get; private set; }
    
    public OperationType Type { get; private set; }

    public string Name { get; private set; }
}