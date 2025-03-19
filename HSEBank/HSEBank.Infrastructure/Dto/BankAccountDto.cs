namespace HSEBank.Infrastructure.Dto;

public struct BankAccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}