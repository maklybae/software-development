using HSEBank.Entities.Core;
using HSEBank.Infrastructure.Repositories.Cached;
using HSEBank.UseCases.Repository;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace HSEBank.Tests.Repository.Cached;

public class CachedAccountsRepositoryTest
{
    private readonly IAccountsRepository _repositoryMock = Substitute.For<IAccountsRepository>();
    private readonly IMemoryCache _cacheMock = Substitute.For<IMemoryCache>();
    private readonly CachedAccountsRepository _cachedRepository;

    public CachedAccountsRepositoryTest()
    {
        _cachedRepository = new CachedAccountsRepository(_repositoryMock, _cacheMock);
    }

    [Fact]
    public void GetById_ShouldReturnCachedAccount_WhenCacheExists()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var cacheKey = $"Account:{accountId}";
        var cachedAccount = new BankAccount("Cached Account", 1000m);

        _cacheMock.TryGetValue(cacheKey, out Arg.Any<BankAccount?>())
            .Returns(x =>
            {
                x[1] = cachedAccount;
                return true;
            });

        // Act
        var result = _cachedRepository.GetById(accountId);

        // Assert
        Assert.Equal(cachedAccount, result);
        _repositoryMock.DidNotReceive().GetById(accountId);
    }

    [Fact]
    public void GetAll_ShouldReturnCachedAccounts_WhenCacheExists()
    {
        // Arrange
        var cachedAccounts = new List<BankAccount>
        {
            new BankAccount("Cached Account 1", 1000m),
            new BankAccount("Cached Account 2", 2000m)
        };

        _cacheMock.TryGetValue("AllAccounts", out Arg.Any<IEnumerable<BankAccount>?>())
            .Returns(x =>
            {
                x[1] = cachedAccounts;
                return true;
            });

        // Act
        var result = _cachedRepository.GetAll();

        // Assert
        Assert.Equal(cachedAccounts, result);
        _repositoryMock.DidNotReceive().GetAll();
    }

    [Fact]
    public void Add_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);

        // Act
        _cachedRepository.Add(account);

        // Assert
        _repositoryMock.Received(1).Add(account);
        _cacheMock.Received(1).Remove("AllAccounts");
    }

    [Fact]
    public void Update_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var account = new BankAccount("Test Account", 1000m);

        // Act
        _cachedRepository.Update(account);

        // Assert
        _repositoryMock.Received(1).Update(account);
        _cacheMock.Received(1).Remove("AllAccounts");
    }

    [Fact]
    public void Delete_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var cacheKey = $"Account:{accountId}";

        // Act
        _cachedRepository.Delete(accountId);

        // Assert
        _repositoryMock.Received(1).Delete(accountId);
        _cacheMock.Received(1).Remove(cacheKey);
        _cacheMock.Received(1).Remove("AllAccounts");
    }

    [Fact]
    public void GetByName_ShouldCallRepositoryDirectly()
    {
        // Arrange
        var name = "Test Account";
        var accounts = new List<BankAccount>
        {
            new BankAccount("Test Account", 1000m)
        };

        _repositoryMock.GetByName(name).Returns(accounts);

        // Act
        var result = _cachedRepository.GetByName(name);

        // Assert
        Assert.Equal(accounts, result);
        _repositoryMock.Received(1).GetByName(name);
        _cacheMock.DidNotReceive().TryGetValue(Arg.Any<string>(), out Arg.Any<object?>());
    }
}