using HSEBank.Entities.Core;

namespace HSEBank.Infrastructure.Dto;

public struct CategoryDto
{
    public Guid Id { get; set; }
    public OperationType Type { get; set; }
    public string? Name { get; set; }
}