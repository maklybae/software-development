namespace HSEBank.Entities.Visitors;

public interface ICoreEntityVisitable
{
    void Accept(ICoreEntityVisitor visitor);
}