using System.Text;
using HSEBank.Entities.Core;
using HSEBank.Infrastructure;
using HSEBank.UseCases.Facades;
using NSubstitute;

namespace HSEBank.Tests;

public class ImporterTest
{
    private readonly IBankAccountFacade _bankAccountFacadeMock = Substitute.For<IBankAccountFacade>();
    private readonly IOperationFacade _operationFacadeMock = Substitute.For<IOperationFacade>();
    private readonly ICategoryFacade _categoryFacadeMock = Substitute.For<ICategoryFacade>();
    private readonly Importer _importer;

    public ImporterTest()
    {
        _importer = new Importer(
            _bankAccountFacadeMock,
            _operationFacadeMock,
            _categoryFacadeMock
        );
    }
    
    [Fact]
    public void Import_ShouldThrowNotSupportedException_ForUnsupportedFormat()
    {
        // Arrange
        var unsupportedFilePath = "test.unsupported";

        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(() => _importer.Import(unsupportedFilePath));
        Assert.Equal("Unsupported file format", exception.Message);
    }

    [Fact]
    public void OpenReader_ShouldThrowIOException_WhenFileCannotBeOpened()
    {
        // Arrange
        var invalidPath = "invalid/path/test.json";

        // Act & Assert
        var exception = Assert.Throws<IOException>(() => _importer.Import(invalidPath));
        Assert.Equal("Failed to open file for reading", exception.Message);
    }
}