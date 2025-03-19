using HSEBank.Entities.Core;

namespace HSEBank.Entities.Visitors;

public interface ICoreEntityVisitor
{
    void Visit(BankAccount bankAccount);
    void Visit(Category category);
    void Visit(Operation operation);
}