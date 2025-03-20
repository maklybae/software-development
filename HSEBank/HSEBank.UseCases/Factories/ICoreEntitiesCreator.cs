using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Factories;

public interface ICoreEntitiesCreator
{
    BankAccount CreateBankAccount(string name, decimal balance);
    BankAccount CreateBankAccount(Guid id, string name, decimal balance);
    Operation CreateOperation(OperationType type,
        BankAccount account,
        decimal amount,
        DateTime date,
        string description,
        Category? category);
    Operation CreateOperation(Guid id,
        OperationType type,
        BankAccount account,
        decimal amount,
        DateTime date,
        string description,
        Category? category);
    Category CreateCategory(OperationType type, string name);
    Category CreateCategory(Guid id, OperationType type, string name);
}