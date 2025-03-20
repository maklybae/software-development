using System.Globalization;
using HSEBank.Entities.Core;
using HSEBank.UseCases.Analytics;
using HSEBank.UseCases.Commands;
using HSEBank.UseCases.Facades;
using Spectre.Console;

namespace HSEBank.CLI;

public class ConsoleApplication
{
    private readonly ICategoryFacade _categoryFacade;
    private readonly IBankAccountFacade _bankAccountFacade;
    private readonly IOperationFacade _operationFacade;
    private readonly IAnalyticsService _analyticsService;
    private bool _isRunning = true;
    
    private struct MenuAction
    {
        public const string AddBankAccount = "Add BankAccount";
        public const string ShowBankAccounts = "Show BankAccounts";
        public const string AddOperation = "Add Operation";
        public const string ShowOperations = "Show Operations";
        public const string AddCategory = "Add Category";
        public const string ShowCategories = "Show Categories";
        public const string DifferenceForAccount = "Difference for Account";
        public const string GroupOperationsByCategory = "Group Operations by Category";
        public const string ExportToJson = "Export to JSON";
        public const string ExportToYaml = "Export to YAML";
        public const string ImportFromJson = "Import from JSON";
        public const string ImportFromYaml = "Import from YAML";
        public const string Exit = "Exit";
    }
    
    public ConsoleApplication(ICategoryFacade categoryFacade, IBankAccountFacade bankAccountFacade, IOperationFacade operationFacade, IAnalyticsService analyticsService)
    {
        _categoryFacade = categoryFacade;
        _bankAccountFacade = bankAccountFacade;
        _operationFacade = operationFacade;
        _analyticsService = analyticsService;
    }

    public void Run()
    {
        while (_isRunning)
        {
            var action = AskAction();
            HandleAction(action);
        }
    }
    
    private void HandleAction(string action)
    {
        switch (action)
        {
            case MenuAction.AddBankAccount:
                AddBankAccount();
                break;
            case MenuAction.ShowBankAccounts:
                ShowBankAccounts();
                break;
            case MenuAction.AddOperation:
                break;
            case MenuAction.ShowOperations:
                break;
            case MenuAction.AddCategory:
                break;
            case MenuAction.ShowCategories:
                break;
            case MenuAction.DifferenceForAccount:
                break;
            case MenuAction.GroupOperationsByCategory:
                break;
            case MenuAction.ExportToJson:
                break;
            case MenuAction.ExportToYaml:
                break;
            case MenuAction.ImportFromJson:
                break;
            case MenuAction.ImportFromYaml:
                break;
            case MenuAction.Exit:
                AnsiConsole.MarkupLine("[red]Goodbye![/]");
                _isRunning = false;
                break;
        }
    }

    private void ShowBankAccounts()
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Balance");

        IEnumerable<BankAccount> accounts;
        try
        {
            accounts = _bankAccountFacade.GetAll();
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            return;
        }
        
        foreach (var bankAccount in accounts)
        {
            table.AddRow(bankAccount.Id.ToString(), bankAccount.Name.ToString(), bankAccount.Balance.ToString(CultureInfo.InvariantCulture));
        }
        AnsiConsole.Write(table);
    }

    private void AddBankAccount()
    {
        var name = AnsiConsole.Ask<string>("Enter [green]name[/]:");
        var balance = AnsiConsole.Ask<decimal>("Enter [green]balance[/]:");

        var command = CompositionRoot.CreateBankAccountCommand;
        command.Create(name, balance);
        var timingCommand = new TimingCommand(command);

        try
        {
            timingCommand.Execute();
            AnsiConsole.MarkupLine("[green]Bank account created successfully![/]");
            AnsiConsole.MarkupLine($"[green]Execution time: {timingCommand.Duraion}[/]");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }
    
    // private void AddOperation()
    // {
    //     var bankAccountName = AnsiConsole.Ask<string>("Enter [green]bank account name[/]:");
    //     var categoryName = AnsiConsole.Ask<string>("Enter [green]category name[/]:");
    //     var amount = AnsiConsole.Ask<decimal>("Enter [green]amount[/]:");
    //     var operationType = AnsiConsole.Prompt(
    //         new SelectionPrompt<OperationType>()
    //             .Title("Select [green]operation type[/]")
    //             .AddChoices(new[]
    //             {
    //                 OperationType.Income, OperationType.Expense
    //             }));
    //     
    //     var command = CompositionRoot.CreateOperationCommand;
    //     command.Create(bankAccountName, categoryName, amount, operationType);
    //     var timingCommand = new TimingCommand(command);
    //
    //     try
    //     {
    //         timingCommand.Execute();
    //         AnsiConsole.MarkupLine("[green]Operation created successfully![/]");
    //         AnsiConsole.MarkupLine($"[green]Execution time: {timingCommand.Duraion}[/]");
    //     }
    //     catch (Exception e)
    //     {
    //         AnsiConsole.WriteException(e);
    //     }
    // }

    private static string AskAction()
    {
        var action = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .PageSize(8)
                .Title("Select [green]action[/]?")
                .MoreChoicesText("[grey](Move up and down to reveal more actions)[/]")
                .AddChoiceGroup("Entities", new[]
                {
                    MenuAction.AddBankAccount, MenuAction.ShowBankAccounts, MenuAction.AddOperation, MenuAction.ShowOperations, MenuAction.AddCategory, MenuAction.ShowCategories
                })
                .AddChoiceGroup("Analytics", new[]
                {
                    MenuAction.DifferenceForAccount, MenuAction.GroupOperationsByCategory
                })
                .AddChoiceGroup("Export", new[]
                {
                    MenuAction.ExportToJson, MenuAction.ExportToYaml
                })
                .AddChoiceGroup("Import", new[]
                {
                    MenuAction.ImportFromJson, MenuAction.ImportFromYaml
                })
                .AddChoices(new[]
                {
                    MenuAction.Exit
                }));
        
        return action;
    }
    
    
}