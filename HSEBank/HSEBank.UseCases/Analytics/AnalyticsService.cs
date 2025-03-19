using HSEBank.Entities.Core;
using HSEBank.UseCases.Facades;
using HSEBank.UseCases.Repository;

namespace HSEBank.UseCases.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IOperationFacade _operationFacade;
    private readonly IAccountsRepository _accountsRepository;
    private readonly ICategoryFacade _categoryFacade;
    
    public AnalyticsService(IOperationFacade operationFacade, IAccountsRepository accountsRepository, ICategoryFacade categoryFacade)
    {
        _operationFacade = operationFacade;
        _accountsRepository = accountsRepository;
        _categoryFacade = categoryFacade;
    }

    public decimal GetDifferenceByAccountId(Guid accountId, DateTime startDate, DateTime endDate)
    {
        var operations = _operationFacade.GetByAccountId(accountId).ToList();
        
        var income = operations.Where(o => o.Type == OperationType.Income && o.Date >= startDate && o.Date <= endDate).Sum(o => o.Amount);
        var expense = operations.Where(o => o.Type == OperationType.Expense && o.Date >= startDate && o.Date <= endDate).Sum(o => o.Amount);
        
        return income - expense;
    }
    
    public Dictionary<Category, decimal> GroupOperationsByCategory(Guid accountId, DateTime startDate, DateTime endDate)
    {
        var operations = _operationFacade.GetByAccountId(accountId).ToList();
        var result = new Dictionary<Category, decimal>();

        foreach (var operation in operations)
        {
            if (operation.Date >= startDate && operation.Date <= endDate)
            {
                if (operation.Category == null) continue;
                if (result.ContainsKey(operation.Category))
                {
                    result[operation.Category] += operation.Amount;
                }
                else
                {
                    result.Add(operation.Category, operation.Amount);
                }
            }
        }

        return result;
    }
}