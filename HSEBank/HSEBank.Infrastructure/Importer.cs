using System.Text.Json;
using HSEBank.Infrastructure.Dto;
using HSEBank.UseCases.Facades;
using HSEBank.UseCases.Import;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HSEBank.Infrastructure;

public class Importer : IImporter
{
    private readonly IBankAccountFacade _bankAccountFacade;
    private readonly IOperationFacade _operationFacade;
    private readonly ICategoryFacade _categoryFacade;

    public Importer(IBankAccountFacade bankAccountFacade, IOperationFacade operationFacade, ICategoryFacade categoryFacade)
    {
        _bankAccountFacade = bankAccountFacade;
        _operationFacade = operationFacade;
        _categoryFacade = categoryFacade;
    }

    public void Import(string path)
    {
        var extension = Path.GetExtension(path);

        Stream stream;
        BankDto dto;
        if (extension.Equals(".yaml", StringComparison.CurrentCultureIgnoreCase))
        {
            stream = OpenReader(path);
            dto = ReadYaml(stream);
        }
        else if (extension.Equals(".json", StringComparison.CurrentCultureIgnoreCase))
        {
            stream = OpenReader(path);
            dto = ReadJson(stream);
        }
        else
        {
            throw new NotSupportedException("Unsupported file format");
        }
        stream.Close();
        
        ImportBank(dto);
    }
    
    private Stream OpenReader(string path)
    {
        try
        {
            return new FileStream(path, FileMode.Open);
        }
        catch (Exception e)
        {
            // Wraps all exceptions into IOException
            throw new IOException("Failed to open file for reading", e);
        }
    }

    private BankDto ReadJson(Stream stream)
    {
        using var reader = new StreamReader(stream);
        string json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<BankDto>(json);
    }
    
    private BankDto ReadYaml(Stream stream)
    {
        using var reader = new StreamReader(stream);
        string yaml = reader.ReadToEnd();
        var deserializer = new DeserializerBuilder().Build();
        return deserializer.Deserialize<BankDto>(yaml);
    }
    
    private void ImportBank(BankDto dto)
    {
        foreach (var bankAccount in dto.BankAccounts)
        {
            _bankAccountFacade.CreatePost(
                bankAccount.Id,
                bankAccount.Name,
                bankAccount.Balance
            );
        }
        
        foreach (var category in dto.Categories)
        {
            _categoryFacade.CreatePost(
                category.Id,
                category.Name ?? "",
                category.Type
            );
        }
        
        foreach (var operation in dto.Operations)
        {
            _operationFacade.CreatePostByIds(
                operation.Id,
                operation.Type,
                operation.BankAccountId,
                operation.Amount,
                operation.Date,
                operation.Description ?? "",
                operation.CategoryId
            );
        }
    }
}