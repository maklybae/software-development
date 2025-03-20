using HSEBank.Entities.Core;
using HSEBank.Infrastructure.Repositories.InMemory;

namespace HSEBank.Tests.Repository.InMemory;

public class InMemoryAccountsRepositoryTest
{
    private readonly InMemoryAccountsRepository _repository = new();

    [Fact]
    public void Add_ShouldAddAccountToRepository()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);

        // Act
        _repository.Add(account);

        // Assert
        var retrievedAccount = _repository.GetById(account.Id);
        Assert.Equal(account, retrievedAccount);
    }

    [Fact]
    public void Add_ShouldThrowException_WhenAccountIsNull()
    {
        // Arrange
        BankAccount account = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.Add(account));
    }

    [Fact]
    public void Add_ShouldThrowException_WhenAccountAlreadyExists()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        _repository.Add(account);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Add(account));
        Assert.Equal("Account already exists", exception.Message);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectAccount()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        _repository.Add(account);

        // Act
        var result = _repository.GetById(account.Id);

        // Assert
        Assert.Equal(account, result);
    }

    [Fact]
    public void GetById_ShouldThrowException_WhenAccountNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.GetById(id));
        Assert.Equal("Account not found", exception.Message);
    }

    [Fact]
    public void GetAll_ShouldReturnAllAccounts()
    {
        // Arrange
        var account1 = new BankAccount("Account 1", 1000m);
        var account2 = new BankAccount("Account 2", 2000m);
        _repository.Add(account1);
        _repository.Add(account2);

        // Act
        var result = _repository.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(account1, result);
        Assert.Contains(account2, result);
    }

    [Fact]
    public void Update_ShouldUpdateAccount()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        _repository.Add(account);

        var updatedAccount = new BankAccount(account.Id, "Updated Account", 2000m);

        // Act
        _repository.Update(updatedAccount);

        // Assert
        var result = _repository.GetById(account.Id);
        Assert.Equal(updatedAccount.Name, result.Name);
        Assert.Equal(updatedAccount.Balance, result.Balance);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenAccountIsNull()
    {
        // Arrange
        BankAccount account = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.Update(account));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenAccountNotFound()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Update(account));
        Assert.Equal("Account not found", exception.Message);
    }

    [Fact]
    public void Delete_ShouldRemoveAccount()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        _repository.Add(account);

        // Act
        _repository.Delete(account.Id);

        // Assert
        Assert.Throws<InvalidOperationException>(() => _repository.GetById(account.Id));
    }

    [Fact]
    public void Delete_ShouldThrowException_WhenAccountNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Delete(id));
        Assert.Equal("Account not found", exception.Message);
    }

    [Fact]
    public void GetByName_ShouldReturnAccountsWithMatchingName()
    {
        // Arrange
        var account1 = new BankAccount("Test Account", 1000m);
        var account2 = new BankAccount("Another Account", 2000m);
        var account3 = new BankAccount("Test Account", 3000m);
        _repository.Add(account1);
        _repository.Add(account2);
        _repository.Add(account3);

        // Act
        var result = _repository.GetByName("Test Account").ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(account1, result);
        Assert.Contains(account3, result);
    }

    [Fact]
    public void GetByName_ShouldThrowException_WhenNameIsNull()
    {
        // Arrange
        string name = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.GetByName(name));
    }

    [Fact]
    public void GetByName_ShouldReturnEmptyList_WhenNoMatchingAccounts()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        _repository.Add(account);

        // Act
        var result = _repository.GetByName("Non-Existent Account").ToList();

        // Assert
        Assert.Empty(result);
    }
}