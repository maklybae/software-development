using HSEBank.Entities.Core;
using HSEBank.UseCases.DataSources;

namespace HSEBank.UseCases.Export;

// Needs to demonstrate Visitor pattern.
public class CoreEntitiesAggregator(
    IBankAccountsGetter accountsGetter,
    ICategoriesGetter categoriesGetter,
    IOperationsGetter operationsGetter) : ICoreEntitiesAggregator
{
    public IEnumerable<ICoreEntity> GetAll()
    {
        var accounts = accountsGetter.GetAll();
        var categories = categoriesGetter.GetAll();
        var operations = operationsGetter.GetAll();

        return accounts
            .Concat(categories.Cast<ICoreEntity>())
            .Concat(operations);
    }
}