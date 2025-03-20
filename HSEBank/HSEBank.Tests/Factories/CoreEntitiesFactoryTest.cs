using HSEBank.Entities.Core;
using HSEBank.UseCases.Factories;

namespace HSEBank.Tests.Factories;

public class CoreEntitiesFactoryTest
{
    private readonly CoreEntitiesFactory _factory = new CoreEntitiesFactory();

    [Fact]
    public void CreateBankAccount_ShouldCreateBankAccountWithNameAndBalance()
    {
        // Arrange
        var name = "Test Account";
        var balance = 1000m;

        // Act
        var result = _factory.CreateBankAccount(name, balance);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(balance, result.Balance);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public void CreateBankAccount_WithId_ShouldCreateBankAccountWithIdNameAndBalance()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Account";
        var balance = 1000m;

        // Act
        var result = _factory.CreateBankAccount(id, name, balance);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(balance, result.Balance);
    }

    [Fact]
    public void CreateOperation_ShouldCreateOperationWithAllParameters()
    {
        // Arrange
        var type = OperationType.Income;
        var account = new BankAccount("Test Account", 1000m);
        var amount = 500m;
        var date = DateTime.UtcNow;
        var description = "Test Description";
        var category = new Category(OperationType.Income, "Test Category");

        // Act
        var result = _factory.CreateOperation(type, account, amount, date, description, category);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(type, result.Type);
        Assert.Equal(account, result.BankAccount);
        Assert.Equal(amount, result.Amount);
        Assert.Equal(date, result.Date);
        Assert.Equal(description, result.Description);
        Assert.Equal(category, result.Category);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public void CreateOperation_WithId_ShouldCreateOperationWithIdAndAllParameters()
    {
        // Arrange
        var id = Guid.NewGuid();
        var type = OperationType.Income;
        var account = new BankAccount("Test Account", 1000m);
        var amount = 500m;
        var date = DateTime.UtcNow;
        var description = "Test Description";
        var category = new Category(OperationType.Income, "Test Category");

        // Act
        var result = _factory.CreateOperation(id, type, account, amount, date, description, category);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(type, result.Type);
        Assert.Equal(account, result.BankAccount);
        Assert.Equal(amount, result.Amount);
        Assert.Equal(date, result.Date);
        Assert.Equal(description, result.Description);
        Assert.Equal(category, result.Category);
    }

    [Fact]
    public void CreateCategory_ShouldCreateCategoryWithTypeAndName()
    {
        // Arrange
        var type = OperationType.Income;
        var name = "Test Category";

        // Act
        var result = _factory.CreateCategory(type, name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(type, result.Type);
        Assert.Equal(name, result.Name);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public void CreateCategory_WithId_ShouldCreateCategoryWithIdTypeAndName()
    {
        // Arrange
        var id = Guid.NewGuid();
        var type = OperationType.Income;
        var name = "Test Category";

        // Act
        var result = _factory.CreateCategory(id, type, name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(type, result.Type);
        Assert.Equal(name, result.Name);
    }
}