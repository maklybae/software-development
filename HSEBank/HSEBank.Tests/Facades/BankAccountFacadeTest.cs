using HSEBank.Entities.Core;
using HSEBank.UseCases.Facades;
using HSEBank.UseCases.Factories;
using HSEBank.UseCases.Repository;
using NSubstitute;

namespace HSEBank.Tests.Facades;

public class BankAccountFacadeTest
{
    private readonly IAccountsRepository _accountsRepositoryMock = Substitute.For<IAccountsRepository>();
    private readonly ICoreEntitiesCreator _factoryMock = Substitute.For<ICoreEntitiesCreator>();
    private readonly BankAccountFacade _facade;
    
    public BankAccountFacadeTest()
    {
        _facade = new BankAccountFacade(_accountsRepositoryMock, _factoryMock);
    }

    [Fact]
    public void CreatePost_ShouldCallFactoryAndRepository_WithCorrectParameters()
    {
        // Arrange
        var expectedName = "Test Account";
        var expectedBalance = 1000m;
        var expectedAccount = new BankAccount(expectedName, expectedBalance);
        _factoryMock.CreateBankAccount(expectedName, expectedBalance).Returns(expectedAccount);

        // Act
        var result = _facade.CreatePost(expectedName, expectedBalance);

        // Assert
        _factoryMock.Received(1).CreateBankAccount(expectedName, expectedBalance);
        _accountsRepositoryMock.Received(1).Add(expectedAccount);
        Assert.Equal(expectedAccount, result);
    }
    
    [Fact]
    public void GetById_ShouldReturnCorrectAccount()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var expectedAccount = new BankAccount("Test", 500m);
        _accountsRepositoryMock.GetById(accountId).Returns(expectedAccount);

        // Act
        var result = _facade.GetById(accountId);

        // Assert
        Assert.Equal(expectedAccount, result);
    }
    
    [Fact]
    public void GetUniqueByName_ShouldReturnCorrectAccount()
    {
        // Arrange
        var expectedName = "Unique Account";
        var expectedAccount = new BankAccount(expectedName, 1500m);
        _accountsRepositoryMock.GetByName(expectedName).Returns(new List<BankAccount> { expectedAccount });

        // Act
        var result = _facade.GetUniqueByName(expectedName);

        // Assert
        Assert.Equal(expectedAccount, result);
    }
    
    [Fact]
    public void GetUniqueByName_WhenAccountNotFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var nonExistentName = "Nonexistent";
        _accountsRepositoryMock.GetByName(nonExistentName).Returns(Enumerable.Empty<BankAccount>());

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _facade.GetUniqueByName(nonExistentName));
    }
    
    [Fact]
    public void UpdateNameById_ShouldUpdateAccountName()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new BankAccount("Old Name", 100m);
        _accountsRepositoryMock.GetById(accountId).Returns(account);

        // Act
        _facade.UpdateNameById(accountId, "New Name");

        // Assert
        Assert.Equal("New Name", account.Name);
        _accountsRepositoryMock.Received(1).Update(account);
    }
    
    [Fact]
    public void IncreaseBalanceById_ShouldIncreaseBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new BankAccount("Test", 500m);
        _accountsRepositoryMock.GetById(accountId).Returns(account);

        // Act
        _facade.IncreaseBalanceById(accountId, 200m);

        // Assert
        Assert.Equal(700m, account.Balance);
        _accountsRepositoryMock.Received(1).Update(account);
    }
    
    [Fact]
    public void DecreaseBalanceById_ShouldDecreaseBalance()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new BankAccount("Test", 500m);
        _accountsRepositoryMock.GetById(accountId).Returns(account);

        // Act
        _facade.DecreaseBalanceById(accountId, 200m);

        // Assert
        Assert.Equal(300m, account.Balance);
        _accountsRepositoryMock.Received(1).Update(account);
    }
    
    [Fact]
    public void Delete_ShouldCallRepositoryDelete()
    {
        // Arrange
        var accountId = Guid.NewGuid();

        // Act
        _facade.Delete(accountId);

        // Assert
        _accountsRepositoryMock.Received(1).Delete(accountId);
    }
}