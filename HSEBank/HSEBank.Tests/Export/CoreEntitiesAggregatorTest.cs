using HSEBank.Entities.Core;
using HSEBank.UseCases.DataSources;
using HSEBank.UseCases.Export;
using NSubstitute;

namespace HSEBank.Tests.Export;

public class CoreEntitiesAggregatorTest
{
    private readonly IBankAccountsGetter _accountsGetterMock = Substitute.For<IBankAccountsGetter>();
    private readonly ICategoriesGetter _categoriesGetterMock = Substitute.For<ICategoriesGetter>();
    private readonly IOperationsGetter _operationsGetterMock = Substitute.For<IOperationsGetter>();
    private readonly CoreEntitiesAggregator _aggregator;

    public CoreEntitiesAggregatorTest()
    {
        _aggregator = new CoreEntitiesAggregator(_accountsGetterMock, _categoriesGetterMock, _operationsGetterMock);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEntitiesFromGetters()
    {
        // Arrange
        var accounts = new List<BankAccount>
        {
            new BankAccount("Account 1", 1000m),
            new BankAccount("Account 2", 2000m)
        };

        var categories = new List<Category>
        {
            new Category(OperationType.Income, "Category 1"),
            new Category(OperationType.Expense, "Category 2")
        };

        var operations = new List<Operation>
        {
            new Operation(OperationType.Income, accounts[0], 500m, DateTime.UtcNow, "Test Operation 1", categories[0]),
            new Operation(OperationType.Expense, accounts[1], 300m, DateTime.UtcNow, "Test Operation 2", categories[1])
        };

        _accountsGetterMock.GetAll().Returns(accounts);
        _categoriesGetterMock.GetAll().Returns(categories);
        _operationsGetterMock.GetAll().Returns(operations);

        // Act
        var result = _aggregator.GetAll().ToList();

        // Assert
        Assert.Equal(accounts.Count + categories.Count + operations.Count, result.Count);
        Assert.Contains(accounts[0], result);
        Assert.Contains(accounts[1], result);
        Assert.Contains(categories[0], result);
        Assert.Contains(categories[1], result);
        Assert.Contains(operations[0], result);
        Assert.Contains(operations[1], result);

        _accountsGetterMock.Received(1).GetAll();
        _categoriesGetterMock.Received(1).GetAll();
        _operationsGetterMock.Received(1).GetAll();
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyList_WhenNoEntitiesExist()
    {
        // Arrange
        _accountsGetterMock.GetAll().Returns(new List<BankAccount>());
        _categoriesGetterMock.GetAll().Returns(new List<Category>());
        _operationsGetterMock.GetAll().Returns(new List<Operation>());

        // Act
        var result = _aggregator.GetAll().ToList();

        // Assert
        Assert.Empty(result);

        _accountsGetterMock.Received(1).GetAll();
        _categoriesGetterMock.Received(1).GetAll();
        _operationsGetterMock.Received(1).GetAll();
    }

}