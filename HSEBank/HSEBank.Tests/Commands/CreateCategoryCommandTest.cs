using HSEBank.Entities.Core;
using HSEBank.UseCases.Commands;
using HSEBank.UseCases.Facades;
using NSubstitute;

namespace HSEBank.Tests.Commands;

public class CreateCategoryCommandTest
{
    private readonly ICategoryFacade _categoryFacadeMock = Substitute.For<ICategoryFacade>();
    
    [Fact]
    public void Execute_ShouldCallFacadeCreatePost_WithCorrectParameters()
    {
        // Arrange
        var command = new CreateCategoryCommand(_categoryFacadeMock);
        var expectedName = "Test Category";
        var expectedType = OperationType.Expense;
        
        // Act
        command.Create(expectedName, expectedType);
        command.Execute();
        
        // Assert
        _categoryFacadeMock.Received(1).CreatePost(expectedName, expectedType);
    }
    
    [Fact]
    public void Execute_WhenNotCreated_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateCategoryCommand(_categoryFacadeMock);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }
}