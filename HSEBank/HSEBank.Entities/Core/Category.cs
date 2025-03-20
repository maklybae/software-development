using HSEBank.Entities.Visitors;

namespace HSEBank.Entities.Core;

public class Category : ICoreEntity
{
    private string _name;
    
    public Guid Id { get; private set; }
    
    // Immutable Type to maintain Category/Type invariant in Operation.
    public OperationType Type { get; private init; }

    public string Name {
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
    
    public Category(Guid id, OperationType type, string name)
    {
        Id = id;
        Type = type;
        Name = name;
    }

    
    public Category(OperationType type, string name)
    {
        Type = type;
        Name = name;
        Id = Guid.NewGuid();
    }

    public void Accept(ICoreEntityVisitor visitor)
    {
        visitor.Visit(this);
    }
}