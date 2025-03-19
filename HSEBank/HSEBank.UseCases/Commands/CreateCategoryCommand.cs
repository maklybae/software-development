using HSEBank.Entities.Core;
using HSEBank.UseCases.Facades;

namespace HSEBank.UseCases.Commands;

public class CreateCategoryCommand : ICommand
{
    private readonly ICategoryFacade _categoryFacade;
    private string _name;
    private OperationType _type;
    
    public CreateCategoryCommand(ICategoryFacade categoryFacade)
    {
        _categoryFacade = categoryFacade;
    }

    public void Create(string name, OperationType type)
    {
        _name = name;
        _type = type;
    }
    
    public void Execute()
    {
        _categoryFacade.CreatePostCategory(_name, _type);
    }
}