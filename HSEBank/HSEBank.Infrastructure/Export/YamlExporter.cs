using HSEBank.Entities.Core;
using HSEBank.Entities.Visitors;
using HSEBank.Infrastructure.Dto;
using HSEBank.UseCases.Export;

namespace HSEBank.Infrastructure.Export;

public sealed class YamlExporter : Exporter, ICoreEntityVisitor
{
    public YamlExporter(ICoreEntitiesAggregator aggregator) : base(aggregator) {}
    
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
        YamlDotNet.Serialization.Serializer serializer = new YamlDotNet.Serialization.Serializer();
        var writer = new StreamWriter(stream);
        serializer.Serialize(writer, new BankDto
        {
            BankAccounts = _bankAccounts,
            Categories = _categories,
            Operations = _operations
        });
        writer.Flush();
    }

    public void Visit(BankAccount bankAccount)
    {
        // Potential different logic for Yaml
        _bankAccounts.Add(new BankAccountDto
        {
            Id = bankAccount.Id,
            Name = bankAccount.Name,
            Balance = bankAccount.Balance
        });
    }

    public void Visit(Category category)
    {
        // Potential different logic for Yaml
        _categories.Add(new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Type = category.Type
        });
    }

    public void Visit(Operation operation)
    {
        // Potential different logic for Yaml
        _operations.Add(new OperationDto
        {
            Id = operation.Id,
            Type = operation.Type,
            Amount = operation.Amount,
            Date = operation.Date,
            Description = operation.Description,
            CategoryId = operation.Category?.Id,
            BankAccountId = operation.BankAccount.Id
        });
    }
}