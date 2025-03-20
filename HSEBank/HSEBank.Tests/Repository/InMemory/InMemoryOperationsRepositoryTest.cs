using HSEBank.Entities.Core;
using HSEBank.Infrastructure.Repositories.InMemory;

namespace HSEBank.Tests.Repository.InMemory;

public class InMemoryOperationsRepositoryTest
{
    private readonly InMemoryOperationsRepository _repository = new();

    [Fact]
    public void Add_ShouldAddOperationToRepository()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);

        // Act
        _repository.Add(operation);

        // Assert
        var retrievedOperation = _repository.GetById(operation.Id);
        Assert.Equal(operation, retrievedOperation);
    }

    [Fact]
    public void Add_ShouldThrowException_WhenOperationIsNull()
    {
        // Arrange
        Operation operation = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.Add(operation));
    }

    [Fact]
    public void Add_ShouldThrowException_WhenOperationAlreadyExists()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);
        _repository.Add(operation);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Add(operation));
        Assert.Equal("Operation already exists", exception.Message);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectOperation()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);
        _repository.Add(operation);

        // Act
        var result = _repository.GetById(operation.Id);

        // Assert
        Assert.Equal(operation, result);
    }

    [Fact]
    public void GetById_ShouldThrowException_WhenOperationNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.GetById(id));
        Assert.Equal("Operation not found", exception.Message);
    }

    [Fact]
    public void GetAll_ShouldReturnAllOperations()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation1 = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Operation 1", null);
        var operation2 = new Operation(OperationType.Expense, account, 200m, DateTime.UtcNow, "Operation 2", null);
        _repository.Add(operation1);
        _repository.Add(operation2);

        // Act
        var result = _repository.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(operation1, result);
        Assert.Contains(operation2, result);
    }

    [Fact]
    public void Update_ShouldUpdateOperation()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);
        _repository.Add(operation);

        var updatedOperation = new Operation(operation.Id, OperationType.Expense, account, 300m, DateTime.UtcNow, "Updated Operation", null);

        // Act
        _repository.Update(updatedOperation);

        // Assert
        var result = _repository.GetById(operation.Id);
        Assert.Equal(updatedOperation.Type, result.Type);
        Assert.Equal(updatedOperation.Amount, result.Amount);
        Assert.Equal(updatedOperation.Description, result.Description);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenOperationIsNull()
    {
        // Arrange
        Operation operation = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.Update(operation));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenOperationNotFound()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Update(operation));
        Assert.Equal("Operation not found", exception.Message);
    }

    [Fact]
    public void Delete_ShouldRemoveOperation()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);
        _repository.Add(operation);

        // Act
        _repository.Delete(operation.Id);

        // Assert
        Assert.Throws<InvalidOperationException>(() => _repository.GetById(operation.Id));
    }

    [Fact]
    public void Delete_ShouldThrowException_WhenOperationNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Delete(id));
        Assert.Equal("Operation not found", exception.Message);
    }

    [Fact]
    public void GetByAccount_ShouldReturnOperationsForAccount()
    {
        // Arrange
        var account1 = new BankAccount("Account 1", 1000m);
        var account2 = new BankAccount("Account 2", 2000m);
        var operation1 = new Operation(OperationType.Income, account1, 500m, DateTime.UtcNow, "Operation 1", null);
        var operation2 = new Operation(OperationType.Expense, account2, 200m, DateTime.UtcNow, "Operation 2", null);
        var operation3 = new Operation(OperationType.Income, account1, 300m, DateTime.UtcNow, "Operation 3", null);
        _repository.Add(operation1);
        _repository.Add(operation2);
        _repository.Add(operation3);

        // Act
        var result = _repository.GetByAccount(account1).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(operation1, result);
        Assert.Contains(operation3, result);
    }

    [Fact]
    public void GetByAccount_ShouldThrowException_WhenAccountIsNull()
    {
        // Arrange
        BankAccount account = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.GetByAccount(account));
    }

    [Fact]
    public void GetByAccount_ShouldReturnEmptyList_WhenNoOperationsForAccount()
    {
        // Arrange
        var account1 = new BankAccount("Account 1", 1000m);
        var account2 = new BankAccount("Account 2", 2000m);
        var operation = new Operation(OperationType.Income, account2, 500m, DateTime.UtcNow, "Test Operation", null);
        _repository.Add(operation);

        // Act
        var result = _repository.GetByAccount(account1).ToList();

        // Assert
        Assert.Empty(result);
    }
}