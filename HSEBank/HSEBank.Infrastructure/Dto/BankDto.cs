namespace HSEBank.Infrastructure.Dto;

public struct BankDto
{
    public List<CategoryDto> Categories { get; set; }
    public List<OperationDto> Operations { get; set; }
    public List<BankAccountDto> BankAccounts { get; set; }
}