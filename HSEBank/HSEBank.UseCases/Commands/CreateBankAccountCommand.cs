using HSEBank.UseCases.Facades;

namespace HSEBank.UseCases.Commands;

public class CreateBankAccountCommand : ICommand
{
    private readonly IBankAccountFacade _bankAccountFacade;
    private string _name;
    private decimal _balance;
    
    public CreateBankAccountCommand(IBankAccountFacade bankAccountFacade)
    {
        _bankAccountFacade = bankAccountFacade;
    }

    public void Create(string name, decimal balance)
    {
        _name = name;
        _balance = balance;
    }
    
    public void Execute()
    {
        _bankAccountFacade.CreatePostAccount(_name, _balance);
    }
}