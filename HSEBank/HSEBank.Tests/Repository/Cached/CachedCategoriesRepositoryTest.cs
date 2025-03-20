using HSEBank.Entities.Core;
using HSEBank.Infrastructure.Repositories.Cached;
using HSEBank.UseCases.Repository;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace HSEBank.Tests.Repository.Cached;

public class CachedCategoriesRepositoryTest
{
    private readonly ICategoriesRepository _repositoryMock = Substitute.For<ICategoriesRepository>();
    private readonly IMemoryCache _cacheMock = Substitute.For<IMemoryCache>();
    private readonly CachedCategoriesRepository _cachedRepository;

    public CachedCategoriesRepositoryTest()
    {
        _cachedRepository = new CachedCategoriesRepository(_repositoryMock, _cacheMock);
    }

    [Fact]
    public void GetById_ShouldReturnCachedCategory_WhenCacheExists()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var cacheKey = $"Category:{categoryId}";
        var cachedCategory = new Category(OperationType.Income, "Cached Category");

        _cacheMock.TryGetValue(cacheKey, out Arg.Any<Category?>())
            .Returns(x =>
            {
                x[1] = cachedCategory;
                return true;
            });

        // Act
        var result = _cachedRepository.GetById(categoryId);

        // Assert
        Assert.Equal(cachedCategory, result);
        _repositoryMock.DidNotReceive().GetById(categoryId);
    }

    [Fact]
    public void GetAll_ShouldReturnCachedCategories_WhenCacheExists()
    {
        // Arrange
        var cachedCategories = new List<Category>
        {
            new Category(OperationType.Income, "Cached Category 1"),
            new Category(OperationType.Expense, "Cached Category 2")
        };

        _cacheMock.TryGetValue("AllCategories", out Arg.Any<IEnumerable<Category>?>())
            .Returns(x =>
            {
                x[1] = cachedCategories;
                return true;
            });

        // Act
        var result = _cachedRepository.GetAll();

        // Assert
        Assert.Equal(cachedCategories, result);
        _repositoryMock.DidNotReceive().GetAll();
    }

    [Fact]
    public void Add_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");

        // Act
        _cachedRepository.Add(category);

        // Assert
        _repositoryMock.Received(1).Add(category);
        _cacheMock.Received(1).Remove("AllCategories");
    }

    [Fact]
    public void Update_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");

        // Act
        _cachedRepository.Update(category);

        // Assert
        _repositoryMock.Received(1).Update(category);
        _cacheMock.Received(1).Remove("AllCategories");
    }

    [Fact]
    public void Delete_ShouldCallRepositoryAndInvalidateCache()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var cacheKey = $"Category:{categoryId}";

        // Act
        _cachedRepository.Delete(categoryId);

        // Assert
        _repositoryMock.Received(1).Delete(categoryId);
        _cacheMock.Received(1).Remove(cacheKey);
        _cacheMock.Received(1).Remove("AllCategories");
    }

    [Fact]
    public void GetByName_ShouldCallRepositoryDirectly()
    {
        // Arrange
        var name = "Test Category";
        var categories = new List<Category>
        {
            new Category(OperationType.Income, "Test Category")
        };

        _repositoryMock.GetByName(name).Returns(categories);

        // Act
        var result = _cachedRepository.GetByName(name);

        // Assert
        Assert.Equal(categories, result);
        _repositoryMock.Received(1).GetByName(name);
        _cacheMock.DidNotReceive().TryGetValue(Arg.Any<string>(), out Arg.Any<object?>());
    }

    [Fact]
    public void GetByType_ShouldCallRepositoryDirectly()
    {
        // Arrange
        var type = OperationType.Income;
        var categories = new List<Category>
        {
            new Category(OperationType.Income, "Category 1")
        };

        _repositoryMock.GetByType(type).Returns(categories);

        // Act
        var result = _cachedRepository.GetByType(type);

        // Assert
        Assert.Equal(categories, result);
        _repositoryMock.Received(1).GetByType(type);
        _cacheMock.DidNotReceive().TryGetValue(Arg.Any<string>(), out Arg.Any<object?>());
    }
}