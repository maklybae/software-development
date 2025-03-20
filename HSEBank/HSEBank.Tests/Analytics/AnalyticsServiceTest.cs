using HSEBank.Entities.Core;
using HSEBank.UseCases.Analytics;
using HSEBank.UseCases.Facades;
using HSEBank.UseCases.Repository;
using NSubstitute;

namespace HSEBank.Tests.Analytics;

public class AnalyticsServiceTest
{
    private readonly IOperationFacade _operationFacadeMock = Substitute.For<IOperationFacade>();
    private readonly IAccountsRepository _accountsRepositoryMock = Substitute.For<IAccountsRepository>();
    private readonly ICategoryFacade _categoryFacadeMock = Substitute.For<ICategoryFacade>();
    private readonly AnalyticsService _analyticsService;

    public AnalyticsServiceTest()
    {
        _analyticsService = new AnalyticsService(_operationFacadeMock, _accountsRepositoryMock, _categoryFacadeMock);
    }

    [Fact]
    public void GetDifferenceByAccountId_ShouldReturnCorrectDifference()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        var operations = new List<Operation>
        {
            new Operation(OperationType.Income, new BankAccount("Account 1", 1000m), 500m, new DateTime(2023, 2, 1), "Income 1", null),
            new Operation(OperationType.Expense, new BankAccount("Account 1", 1000m), 200m, new DateTime(2023, 3, 1), "Expense 1", null),
            new Operation(OperationType.Income, new BankAccount("Account 1", 1000m), 300m, new DateTime(2023, 4, 1), "Income 2", null),
            new Operation(OperationType.Expense, new BankAccount("Account 1", 1000m), 100m, new DateTime(2023, 5, 1), "Expense 2", null)
        };

        _operationFacadeMock.GetByAccountId(accountId).Returns(operations);

        // Act
        var result = _analyticsService.GetDifferenceByAccountId(accountId, startDate, endDate);

        // Assert
        Assert.Equal(500m + 300m - 200m - 100m, result); // Income: 500 + 300, Expense: 200 + 100
        _operationFacadeMock.Received(1).GetByAccountId(accountId);
    }

    [Fact]
    public void GetDifferenceByAccountId_ShouldReturnZero_WhenNoOperationsInDateRange()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        var operations = new List<Operation>
        {
            new Operation(OperationType.Income, new BankAccount("Account 1", 1000m), 500m, new DateTime(2022, 12, 31), "Income 1", null),
            new Operation(OperationType.Expense, new BankAccount("Account 1", 1000m), 200m, new DateTime(2024, 1, 1), "Expense 1", null)
        };

        _operationFacadeMock.GetByAccountId(accountId).Returns(operations);

        // Act
        var result = _analyticsService.GetDifferenceByAccountId(accountId, startDate, endDate);

        // Assert
        Assert.Equal(0m, result); // No operations in the date range
        _operationFacadeMock.Received(1).GetByAccountId(accountId);
    }

    [Fact]
    public void GroupOperationsByCategory_ShouldReturnCorrectGrouping()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        var category1 = new Category(OperationType.Income, "Category 1");
        var category2 = new Category(OperationType.Expense, "Category 2");

        var operations = new List<Operation>
        {
            new Operation(OperationType.Income, new BankAccount("Account 1", 1000m), 500m, new DateTime(2023, 2, 1), "Income 1", category1),
            new Operation(OperationType.Expense, new BankAccount("Account 1", 1000m), 200m, new DateTime(2023, 3, 1), "Expense 1", category2),
            new Operation(OperationType.Income, new BankAccount("Account 1", 1000m), 300m, new DateTime(2023, 4, 1), "Income 2", category1),
            new Operation(OperationType.Expense, new BankAccount("Account 1", 1000m), 100m, new DateTime(2023, 5, 1), "Expense 2", category2)
        };

        _operationFacadeMock.GetByAccountId(accountId).Returns(operations);

        // Act
        var result = _analyticsService.GroupOperationsByCategory(accountId, startDate, endDate);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(500m + 300m, result[category1]); // Category 1: 500 + 300
        Assert.Equal(200m + 100m, result[category2]); // Category 2: 200 + 100
        _operationFacadeMock.Received(1).GetByAccountId(accountId);
    }

    [Fact]
    public void GroupOperationsByCategory_ShouldIgnoreOperationsWithoutCategory()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var startDate = new DateTime(2023, 1, 1);
        var endDate = new DateTime(2023, 12, 31);

        var category1 = new Category(OperationType.Income, "Category 1");

        var operations = new List<Operation>
        {
            new Operation(OperationType.Income, new BankAccount("Account 1", 1000m), 500m, new DateTime(2023, 2, 1), "Income 1", category1),
            new Operation(OperationType.Income, new BankAccount("Account 1", 1000m), 300m, new DateTime(2023, 4, 1), "Income 2", null) // No category
        };

        _operationFacadeMock.GetByAccountId(accountId).Returns(operations);

        // Act
        var result = _analyticsService.GroupOperationsByCategory(accountId, startDate, endDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(500m, result[category1]); // Only operations with category are included
        _operationFacadeMock.Received(1).GetByAccountId(accountId);
    }
}