using HSEBank.Entities.Core;
using HSEBank.UseCases.Facades;
using HSEBank.UseCases.Factories;
using HSEBank.UseCases.Repository;
using NSubstitute;

namespace HSEBank.Tests.Facades;

public class OperationFacadeTest
{
    private readonly IOperationsRepository _operationsRepositoryMock = Substitute.For<IOperationsRepository>();
    private readonly IAccountsRepository _accountsRepositoryMock = Substitute.For<IAccountsRepository>();
    private readonly ICategoriesRepository _categoriesRepositoryMock = Substitute.For<ICategoriesRepository>();
    private readonly ICoreEntitiesCreator _factoryMock = Substitute.For<ICoreEntitiesCreator>();
    private readonly OperationFacade _facade;
    
    public OperationFacadeTest()
    {
        _facade = new OperationFacade(_operationsRepositoryMock, _factoryMock, _accountsRepositoryMock, _categoriesRepositoryMock);
    }

    [Fact]
    public void CreatePostByIds_ShouldCreateAndStoreOperation()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var expectedAccount = new BankAccount("Test Account", 1000m);
        var expectedCategory = new Category(OperationType.Income, "Test Category");
        var expectedOperation = new Operation(OperationType.Income, expectedAccount, 500m, DateTime.UtcNow, "Test Description", expectedCategory);
        
        _accountsRepositoryMock.GetById(accountId).Returns(expectedAccount);
        _categoriesRepositoryMock.GetById(categoryId).Returns(expectedCategory);
        _factoryMock.CreateOperation(OperationType.Income, expectedAccount, 500m, expectedOperation.Date, "Test Description", expectedCategory)
            .Returns(expectedOperation);

        // Act
        var result = _facade.CreatePostByIds(OperationType.Income, accountId, 500m, expectedOperation.Date, "Test Description", categoryId);

        // Assert
        _accountsRepositoryMock.Received(1).GetById(accountId);
        _categoriesRepositoryMock.Received(1).GetById(categoryId);
        _operationsRepositoryMock.Received(1).Add(expectedOperation);
        Assert.Equal(expectedOperation, result);
    }
    
    [Fact]
    public void GetById_ShouldReturnCorrectOperation()
    {
        // Arrange
        var operationId = Guid.NewGuid();
        var expectedOperation = new Operation(OperationType.Expense, new BankAccount("Account", 500m), 200m, DateTime.UtcNow, "Test", null);
        _operationsRepositoryMock.GetById(operationId).Returns(expectedOperation);

        // Act
        var result = _facade.GetById(operationId);

        // Assert
        Assert.Equal(expectedOperation, result);
    }
    
    [Fact]
    public void UpdateAmountById_ShouldUpdateOperationAmount()
    {
        // Arrange
        var operationId = Guid.NewGuid();
        var operation = new Operation(OperationType.Income, new BankAccount("Account", 500m), 100m, DateTime.UtcNow, "Test", null);
        _operationsRepositoryMock.GetById(operationId).Returns(operation);

        // Act
        _facade.UpdateAmountById(operationId, 300m);

        // Assert
        Assert.Equal(300m, operation.Amount);
        _operationsRepositoryMock.Received(1).Update(operation);
    }
    
    [Fact]
    public void Delete_ShouldCallRepositoryDelete()
    {
        // Arrange
        var operationId = Guid.NewGuid();

        // Act
        _facade.Delete(operationId);

        // Assert
        _operationsRepositoryMock.Received(1).Delete(operationId);
    }
}