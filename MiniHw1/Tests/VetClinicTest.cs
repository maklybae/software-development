using MiniHw1.Interfaces;
using MiniHw1.Services;
using NSubstitute;

namespace Tests;

public class VetClinicTest
{
    [Theory]
    [InlineData(-100, false)]
    [InlineData(0, false)]
    [InlineData(3, false)]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(10, true)]
    [InlineData(100, false)]
    public void HealthTest_Call_ShouldReturnExpectedResult(int health, bool expected)
    {
        // Arrange
        var vetClinic = new VetClinic();
        var alive = Substitute.For<IAlive>();
        alive.Health.Returns(health);
        
        // Act
        var result = vetClinic.CheckHealth(alive);
        
        // Assert
        Assert.Equal(expected, result);
    }
}