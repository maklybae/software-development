using HSEBank.UseCases.Facades;

namespace HSEBank.UseCases.Commands;

public class CreateBankAccountCommand : ICommand
{
    private readonly IBankAccountFacade _bankAccountFacade;
    private string _name;
    private decimal _balance;
    private bool _isCreated = false;
    
    public CreateBankAccountCommand(IBankAccountFacade bankAccountFacade)
    {
        _bankAccountFacade = bankAccountFacade;
    }

    public void Create(string name, decimal balance)
    {
        _isCreated = true;
        _name = name;
        _balance = balance;
    }
    
    public void Execute()
    {
        if (!_isCreated)
        {
            throw new InvalidOperationException("Command is not created");
        }
        _bankAccountFacade.CreatePost(_name, _balance);
    }
}