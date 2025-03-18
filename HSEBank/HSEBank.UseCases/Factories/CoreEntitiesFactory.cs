using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Factories;

/// <summary>
/// Factory for creating core entities.
/// </summary>
/// <!--All data validation in constructors: user can create Entity bypassing Factory-->
/// <!--Some specific validations can be done in Factory-->
public class CoreEntitiesFactory : ICoreEntitiesCreator
{
    public BankAccount CreateBankAccount(string name, decimal balance = decimal.Zero)
    {
        return new BankAccount(name, balance);
    }

    public Operation CreateOperation(OperationType type, BankAccount account, decimal amount, DateTime date, string description,
        Category? category)
    {
        return new Operation(type, account, amount, date, description, category);
    }

    public Category CreateCategory(OperationType type, string name)
    { 
        return new Category(type, name);
    }
}