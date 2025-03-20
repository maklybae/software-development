using HSEBank.Entities.Core;
using HSEBank.Infrastructure.Repositories.InMemory;

namespace HSEBank.Tests.Repository.InMemory;

public class InMemoryCategoriesRepositoryTest
{
    private readonly InMemoryCategoriesRepository _repository = new InMemoryCategoriesRepository();

    [Fact]
    public void Add_ShouldAddCategoryToRepository()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");

        // Act
        _repository.Add(category);

        // Assert
        var retrievedCategory = _repository.GetById(category.Id);
        Assert.Equal(category, retrievedCategory);
    }

    [Fact]
    public void Add_ShouldThrowException_WhenCategoryAlreadyExists()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");
        _repository.Add(category);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Add(category));
        Assert.Equal("Category already exists", exception.Message);
    }

    [Fact]
    public void GetById_ShouldReturnCorrectCategory()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");
        _repository.Add(category);

        // Act
        var result = _repository.GetById(category.Id);

        // Assert
        Assert.Equal(category, result);
    }

    [Fact]
    public void GetById_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.GetById(id));
        Assert.Equal("Category not found", exception.Message);
    }

    [Fact]
    public void GetAll_ShouldReturnAllCategories()
    {
        // Arrange
        var category1 = new Category(OperationType.Income, "Category 1");
        var category2 = new Category(OperationType.Expense, "Category 2");
        _repository.Add(category1);
        _repository.Add(category2);

        // Act
        var result = _repository.GetAll().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(category1, result);
        Assert.Contains(category2, result);
    }

    [Fact]
    public void Update_ShouldUpdateCategory()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");
        _repository.Add(category);

        var updatedCategory = new Category(category.Id, OperationType.Expense, "Updated Category");

        // Act
        _repository.Update(updatedCategory);

        // Assert
        var result = _repository.GetById(category.Id);
        Assert.Equal(updatedCategory.Type, result.Type);
        Assert.Equal(updatedCategory.Name, result.Name);
    }

    [Fact]
    public void Update_ShouldThrowException_WhenCategoryIsNull()
    {
        // Arrange
        Category category = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _repository.Update(category));
    }

    [Fact]
    public void Update_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Update(category));
        Assert.Equal("Category not found", exception.Message);
    }

    [Fact]
    public void Delete_ShouldRemoveCategory()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");
        _repository.Add(category);

        // Act
        _repository.Delete(category.Id);

        // Assert
        Assert.Throws<InvalidOperationException>(() => _repository.GetById(category.Id));
    }

    [Fact]
    public void Delete_ShouldThrowException_WhenCategoryNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => _repository.Delete(id));
        Assert.Equal("Category not found", exception.Message);
    }

    [Fact]
    public void GetByName_ShouldReturnCategoriesWithMatchingName()
    {
        // Arrange
        var category1 = new Category(OperationType.Income, "Test Category");
        var category2 = new Category(OperationType.Expense, "Another Category");
        var category3 = new Category(OperationType.Income, "Test Category");
        _repository.Add(category1);
        _repository.Add(category2);
        _repository.Add(category3);

        // Act
        var result = _repository.GetByName("Test Category").ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(category1, result);
        Assert.Contains(category3, result);
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
    public void GetByName_ShouldReturnEmptyList_WhenNoMatchingCategories()
    {
        // Arrange
        var category = new Category(OperationType.Income, "Test Category");
        _repository.Add(category);

        // Act
        var result = _repository.GetByName("Non-Existent Category").ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetByType_ShouldReturnCategoriesWithMatchingType()
    {
        // Arrange
        var category1 = new Category(OperationType.Income, "Category 1");
        var category2 = new Category(OperationType.Expense, "Category 2");
        var category3 = new Category(OperationType.Income, "Category 3");
        _repository.Add(category1);
        _repository.Add(category2);
        _repository.Add(category3);

        // Act
        var result = _repository.GetByType(OperationType.Income).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(category1, result);
        Assert.Contains(category3, result);
    }

    [Fact]
    public void GetByType_ShouldReturnEmptyList_WhenNoMatchingCategories()
    {
        // Arrange
        var category1 = new Category(OperationType.Income, "Category 1");
        var category2 = new Category(OperationType.Income, "Category 2");
        _repository.Add(category1);
        _repository.Add(category2);

        // Act
        var result = _repository.GetByType(OperationType.Expense).ToList();

        // Assert
        Assert.Empty(result);
    }
}