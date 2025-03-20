using HSEBank.Entities.Core;
using HSEBank.UseCases.Facades;

namespace HSEBank.UseCases.Commands;

public class CreateOperationCommand : ICommand
{
    private readonly IOperationFacade _operationFacade;
    private OperationType _operationType;
    private Guid _bankAccountId;
    private decimal _amount;
    private DateTime _date;
    private string _description;
    private Guid? _categoryId;
    
    public CreateOperationCommand(IOperationFacade operationFacade)
    {
        _operationFacade = operationFacade;
    }

    public void Create(OperationType operationType, Guid bankAccountId, decimal amount, DateTime date, string description, Guid? categoryId)
    {
        _operationType = operationType;
        _bankAccountId = bankAccountId;
        _amount = amount;
        _date = date;
        _description = description;
        _categoryId = categoryId;
    }
    
    public void Execute()
    {
        _operationFacade.CreatePostByIds(_operationType, _bankAccountId, _amount, _date, _description, _categoryId);
    }
    
}