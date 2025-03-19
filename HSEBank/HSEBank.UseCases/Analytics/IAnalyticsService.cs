using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Analytics;

public interface IAnalyticsService
{
    decimal GetDifferenceByAccountId(Guid accountId, DateTime startDate, DateTime endDate);
    Dictionary<Category, decimal> GroupOperationsByCategory(Guid accountId, DateTime startDate, DateTime endDate);
}