using HSEBank.Entities.Core;

namespace HSEBank.UseCases.Facades;

public interface IOperationFacade
{
    Operation CreatePostOperationByIds(OperationType type, Guid accountId, decimal amount, DateTime date,
        string description,
        Guid categoryId);
    Operation GetById(Guid id);
    IEnumerable<Operation> GetAll();
    void UpdateAccountByIds(Guid operationId, Guid accountId);
    void UpdateAmountById(Guid id, decimal amount);
    void UpdateCategoryToNullById(Guid id);
    void UpdateCategoryByIds(Guid id, Guid categoryId);
    void UpdateDescriptionById(Guid id, string description);
    void UpdateDateById(Guid id, DateTime date);
    void Delete(Guid id);
    
}