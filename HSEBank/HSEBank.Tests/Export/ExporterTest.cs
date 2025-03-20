using HSEBank.UseCases.Export;
using NSubstitute;

namespace HSEBank.Tests.Export;

public class ExporterTest
{
    private readonly ICoreEntitiesAggregator _aggregatorMock = Substitute.For<ICoreEntitiesAggregator>();
    private readonly TestExporter _exporter;

    public ExporterTest()
    {
        _exporter = new TestExporter(_aggregatorMock);
    }

    [Fact]
    public void Export_ShouldCallFormatDataAndWrite()
    {
        // Arrange
        var path = "test_path.txt";

        // Act
        _exporter.Export(path);

        // Assert
        Assert.True(_exporter.FormatDataCalled);
        Assert.NotNull(_exporter.LastStreamUsedForWrite);
    }

    [Fact]
    public void Export_ShouldCloseStreamEvenIfWriteThrowsException()
    {
        // Arrange
        var path = "test_path.txt";
        _exporter.ThrowExceptionInWrite = true;

        // Act & Assert
        Assert.Throws<IOException>(() => _exporter.Export(path));
        Assert.True(_exporter.LastStreamUsedForWrite.CanRead == false); // Stream should be closed
    }

    [Fact]
    public void OpenWriter_ShouldThrowIOException_WhenFileCannotBeCreated()
    {
        // Arrange
        var invalidPath = "invalid/path/test.txt";

        // Act & Assert
        var exception = Assert.Throws<IOException>(() => _exporter.Export(invalidPath));
        Assert.Equal("Failed to open file for writing", exception.Message);
    }

    // Мок-класс для тестирования абстрактного класса Exporter
    private class TestExporter : Exporter
    {
        public bool FormatDataCalled { get; private set; }
        public bool ThrowExceptionInWrite { get; set; }
        public Stream LastStreamUsedForWrite { get; private set; }

        public TestExporter(ICoreEntitiesAggregator aggregator) : base(aggregator)
        {
        }

        protected override void FormatData()
        {
            FormatDataCalled = true;
        }

        protected override void Write(Stream stream)
        {
            LastStreamUsedForWrite = stream;
            if (ThrowExceptionInWrite)
            {
                throw new IOException("Test exception during write");
            }
        }
    }

}