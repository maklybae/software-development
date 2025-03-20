using HSEBank.Entities.Core;
using HSEBank.UseCases.Facades;
using HSEBank.UseCases.Factories;
using HSEBank.UseCases.Repository;
using NSubstitute;

namespace HSEBank.Tests.Facades;

public class CategoryFacadeTest
{
    private readonly ICategoriesRepository _categoriesRepositoryMock = Substitute.For<ICategoriesRepository>();
    private readonly ICoreEntitiesCreator _builderMock = Substitute.For<ICoreEntitiesCreator>();
    private readonly CategoryFacade _facade;

    public CategoryFacadeTest()
    {
        _facade = new CategoryFacade(_categoriesRepositoryMock, _builderMock);
    }

    [Fact]
    public void CreatePost_ShouldCreateAndStoreCategory()
    {
        // Arrange
        var name = "Test Category";
        var operationType = OperationType.Income;
        var expectedCategory = new Category(operationType, name);

        _builderMock.CreateCategory(operationType, name).Returns(expectedCategory);

        // Act
        var result = _facade.CreatePost(name, operationType);

        // Assert
        _builderMock.Received(1).CreateCategory(operationType, name);
        _categoriesRepositoryMock.Received(1).Add(expectedCategory);
        Assert.Equal(expectedCategory, result);
    }

    [Fact]
    public void CreatePost_WithId_ShouldCreateAndStoreCategory()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Category";
        var operationType = OperationType.Income;
        var expectedCategory = new Category(id, operationType, name);

        _builderMock.CreateCategory(id, operationType, name).Returns(expectedCategory);

        // Act
        var result = _facade.CreatePost(id, name, operationType);

        // Assert
        _builderMock.Received(1).CreateCategory(id, operationType, name);
        _categoriesRepositoryMock.Received(1).Add(expectedCategory);
        Assert.Equal(expectedCategory, result);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectCategory()
    {
        // Arrange
        var id = Guid.NewGuid();
        var expectedCategory = new Category(id, OperationType.Income, "Test Category");
        _categoriesRepositoryMock.GetById(id).Returns(expectedCategory);

        // Act
        var result = _facade.GetById(id);

        // Assert
        Assert.Equal(expectedCategory, result);
    }

    [Fact]
    public void GetAll_ShouldReturnAllCategories()
    {
        // Arrange
        var expectedCategories = new List<Category>
        {
            new Category(Guid.NewGuid(),  OperationType.Income, "Category 1"),
            new Category(Guid.NewGuid(), OperationType.Expense, "Category 2")
        };

        _categoriesRepositoryMock.GetAll().Returns(expectedCategories);

        // Act
        var result = _facade.GetAll();

        // Assert
        Assert.Equal(expectedCategories, result);
    }

    [Fact]
    public void UpdateNameById_ShouldUpdateCategoryName()
    {
        // Arrange
        var id = Guid.NewGuid();
        var newName = "Updated Category Name";
        var existingCategory = new Category(id, OperationType.Income, "Test Category");
        _categoriesRepositoryMock.GetById(id).Returns(existingCategory);

        // Act
        _facade.UpdateNameById(id, newName);

        // Assert
        Assert.Equal(newName, existingCategory.Name);
        _categoriesRepositoryMock.Received(1).Update(existingCategory);
    }

    [Fact]
    public void UpdateNameById_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var newName = "Updated Category Name";
        _categoriesRepositoryMock.GetById(id).Returns((Category)null);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _facade.UpdateNameById(id, newName));
        Assert.Equal($"Category with id {id} was not found", exception.Message);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        _facade.Delete(id);

        // Assert
        _categoriesRepositoryMock.Received(1).Delete(id);
    }
}