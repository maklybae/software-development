using HSEBank.Entities.Visitors;

namespace HSEBank.Entities.Core;

public interface ICoreEntity : IIdentifiable, ICoreEntityVisitable
{
}