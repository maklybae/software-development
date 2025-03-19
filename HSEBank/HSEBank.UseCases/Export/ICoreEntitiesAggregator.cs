using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Export;

public interface ICoreEntitiesAggregator
{
    IEnumerable<ICoreEntity> GetAll();
}