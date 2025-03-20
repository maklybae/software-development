using HSEBank.Entities.Core;
using HSEBank.Infrastructure.Repositories.Cached;
using HSEBank.UseCases.Repository;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace HSEBank.Tests.Repository.Cached;

public class CachedOperationsRepositoryTest
{
    private readonly IOperationsRepository _repositoryMock = Substitute.For<IOperationsRepository>();
    private readonly IMemoryCache _cacheMock = Substitute.For<IMemoryCache>();
    private readonly CachedOperationsRepository _cachedRepository;

    public CachedOperationsRepositoryTest()
    {
        _cachedRepository = new CachedOperationsRepository(_repositoryMock, _cacheMock);
    }

    [Fact]
    public void GetById_ShouldReturnCachedOperation_WhenCacheExists()
    {
        // Arrange
        var operationId = Guid.NewGuid();
        var cacheKey = $"Operation:{operationId}";
        var account = new BankAccount("Test Account", 1000m);
        var cachedOperation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Cached Operation", null);

        _cacheMock.TryGetValue(cacheKey, out Arg.Any<Operation?>())
            .Returns(x =>
            {
                x[1] = cachedOperation;
                return true;
            });

        // Act
        var result = _cachedRepository.GetById(operationId);

        // Assert
        Assert.Equal(cachedOperation, result);
        _repositoryMock.DidNotReceive().GetById(operationId);
    }

    [Fact]
    public void GetAll_ShouldReturnCachedOperations_WhenCacheExists()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var cachedOperations = new List<Operation>
        {
            new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Cached Operation 1", null),
            new Operation(OperationType.Expense, account, 200m, DateTime.UtcNow, "Cached Operation 2", null)
        };

        _cacheMock.TryGetValue("AllOperations", out Arg.Any<IEnumerable<Operation>?>())
            .Returns(x =>
            {
                x[1] = cachedOperations;
                return true;
            });

        // Act
        var result = _cachedRepository.GetAll();

        // Assert
        Assert.Equal(cachedOperations, result);
        _repositoryMock.DidNotReceive().GetAll();
    }

    [Fact]
    public void Add_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);

        // Act
        _cachedRepository.Add(operation);

        // Assert
        _repositoryMock.Received(1).Add(operation);
        _cacheMock.Received(1).Remove("AllOperations");
    }

    [Fact]
    public void Update_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operation = new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Test Operation", null);

        // Act
        _cachedRepository.Update(operation);

        // Assert
        _repositoryMock.Received(1).Update(operation);
        _cacheMock.Received(1).Remove("AllOperations");
    }

    [Fact]
    public void Delete_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var operationId = Guid.NewGuid();
        var cacheKey = $"Operation:{operationId}";

        // Act
        _cachedRepository.Delete(operationId);

        // Assert
        _repositoryMock.Received(1).Delete(operationId);
        _cacheMock.Received(1).Remove(cacheKey);
        _cacheMock.Received(1).Remove("AllOperations");
    }

    [Fact]
    public void GetByAccount_ShouldCallRepositoryDirectly()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);
        var operations = new List<Operation>
        {
            new Operation(OperationType.Income, account, 500m, DateTime.UtcNow, "Operation 1", null)
        };

        _repositoryMock.GetByAccount(account).Returns(operations);

        // Act
        var result = _cachedRepository.GetByAccount(account);

        // Assert
        Assert.Equal(operations, result);
        _repositoryMock.Received(1).GetByAccount(account);
        _cacheMock.DidNotReceive().TryGetValue(Arg.Any<string>(), out Arg.Any<object?>());
    }
}