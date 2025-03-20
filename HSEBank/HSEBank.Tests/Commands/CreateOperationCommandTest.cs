using HSEBank.Entities.Core;
using HSEBank.UseCases.Commands;
using HSEBank.UseCases.Facades;
using NSubstitute;

namespace HSEBank.Tests.Commands;

public class CreateOperationCommandTest
{
    private readonly IOperationFacade _operationFacadeMock = Substitute.For<IOperationFacade>();
    
    [Fact]
    public void Execute_ShouldCallFacadeCreatePostByIds_WithCorrectParameters()
    {
        // Arrange
        var command = new CreateOperationCommand(_operationFacadeMock);
        var expectedOperationType = OperationType.Income;
        var expectedBankAccountId = Guid.NewGuid();
        var expectedAmount = 500m;
        var expectedDate = DateTime.UtcNow;
        var expectedDescription = "Test operation";
        var expectedCategoryId = Guid.NewGuid();
        
        // Act
        command.Create(expectedOperationType, expectedBankAccountId, expectedAmount, expectedDate, expectedDescription, expectedCategoryId);
        command.Execute();
        
        // Assert
        _operationFacadeMock.Received(1).CreatePostByIds(
            expectedOperationType,
            expectedBankAccountId,
            expectedAmount,
            expectedDate,
            expectedDescription,
            expectedCategoryId
        );
    }
    
    [Fact]
    public void Execute_WhenNotCreated_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateOperationCommand(_operationFacadeMock);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }
}