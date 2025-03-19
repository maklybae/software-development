namespace HSEBank.Infrastructure.Dto;

public struct OperationDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public Guid BankAccountId { get; set; }
    public Guid? CategoryId { get; set; }
}