using System.Text.Json;
using HSEBank.Entities.Core;
using HSEBank.Entities.Visitors;
using HSEBank.Infrastructure.Dto;
using HSEBank.UseCases.Export;

namespace HSEBank.Infrastructure.Export;

public sealed class JsonExporter : Exporter, ICoreEntityVisitor
{
    public JsonExporter(ICoreEntitiesAggregator aggregator) : base(aggregator) {}
    
    private readonly List<BankAccountDto> _bankAccounts = [];
    private readonly List<CategoryDto> _categories = [];
    private readonly List<OperationDto> _operations = [];
    
    protected override void FormatData()
    {
        foreach (var coreEntity in _aggregator.GetAll())
        {
            coreEntity.Accept(this);
        }
    }

    protected override void Write(Stream stream)
    {
        JsonSerializer.SerializeAsync(stream, new
        {
            BankAccounts = _bankAccounts,
            Categories = _categories,
            Operations = _operations
        }).GetAwaiter().GetResult();
    }

    public void Visit(BankAccount bankAccount)
    {
        // Potential different logic for Json
        _bankAccounts.Add(new  BankAccountDto  
        {
            Id = bankAccount.Id,
            Name = bankAccount.Name,
            Balance = bankAccount.Balance
        });
    }

    public void Visit(Category category)
    {
        // Potential different logic for Json
        _categories.Add(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type
        });
    }

    public void Visit(Operation operation)
    {
        // Potential different logic for Json
        _operations.Add(new OperationDto
        {
            Id = operation.Id,
            Amount = operation.Amount,
            Date = operation.Date,
            Description = operation.Description,
            CategoryId = operation.Category?.Id,
            BankAccountId = operation.BankAccount.Id
        });
    }
}