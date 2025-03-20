using HSEBank.Entities.Core;
using HSEBank.UseCases.Facades;

namespace HSEBank.UseCases.Commands;

public class CreateCategoryCommand : ICommand
{
    private readonly ICategoryFacade _categoryFacade;
    private string _name;
    private OperationType _type;
    private bool _isCreated = false;
    
    public CreateCategoryCommand(ICategoryFacade categoryFacade)
    {
        _categoryFacade = categoryFacade;
    }

    public void Create(string name, OperationType type)
    {
        _isCreated = true;
        _name = name;
        _type = type;
    }
    
    public void Execute()
    {
        if (!_isCreated)
        {
            throw new InvalidOperationException("Command is not created");
        }
        _categoryFacade.CreatePost(_name, _type);
    }
}