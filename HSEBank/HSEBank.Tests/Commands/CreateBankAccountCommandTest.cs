using HSEBank.UseCases.Commands;
using HSEBank.UseCases.Facades;
using NSubstitute;

namespace HSEBank.Tests.Commands;

public class CreateBankAccountCommandTest
{
    private readonly IBankAccountFacade _facadeMock = Substitute.For<IBankAccountFacade>();
    
    [Fact]
    public void Execute_ShouldCallFacadeCreatePost_WithCorrectParameters()
    {
        // Arrange
        var command = new CreateBankAccountCommand(_facadeMock);
        string expectedName = "Test Account";
        decimal expectedBalance = 1000m;

        // Act
        command.Create(expectedName, expectedBalance);
        command.Execute();

        // Assert
        _facadeMock.Received(1).CreatePost(expectedName, expectedBalance);
    }
    
    [Fact]
    public void Execute_WhenNotCreated_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateBankAccountCommand(_facadeMock);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => command.Execute());
    }
}