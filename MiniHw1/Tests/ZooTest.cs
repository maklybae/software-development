using MiniHw1.Interfaces;
using MiniHw1.Models.Animals;
using MiniHw1.Models.Things;
using MiniHw1.Services;
using NSubstitute;

namespace Tests;

public class ZooTest
{
    private class DumbAnimal(string name, int food, int health, int number) : Animal(name, food, health, number)
    {
    }
    
    private class DumbKindAnimal(string name, int food, int health, int number, int kindness) : Animal(name, food, health, number), IKind
    {
        public int Kindness { get; set; } = kindness;
    }
    
    private class DumbThing(int number) : Thing(number)
    {
    }
    
    [Fact]
    public void TryAddAnimal_Call_ShouldAddValid()
    {
        // Arrange
        // Можно было бы создать Mock, но Animal абстрактный класс (по условию) без виртуальных методов
        // Это выглядело бы лучше, будь Animal интерфейсом
        var animal = new DumbAnimal("Test", 0, 0, 0);
        
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        var vetClinic = Substitute.For<IVetClinic>();
        vetClinic.CheckHealth(animal).Returns(true);
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        zoo.TryAddAnimal(animal);
        
        // Assert
        animalsRepository.Received(1).Add(animal);
    }
    
    [Fact]
    public void TryAddAnimal_Call_ShouldNotAddInvalid()
    {
        // Arrange
        var animal = new DumbAnimal("Test", 0, 0, 0);
        
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        var vetClinic = Substitute.For<IVetClinic>();
        vetClinic.CheckHealth(animal).Returns(false);
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        Assert.False(zoo.TryAddAnimal(animal));
        
        // Assert
        animalsRepository.DidNotReceive().Add(animal);
    }

    [Fact]
    public void Animals_Call_ShouldReturnExpectedValue()
    {
        // Arrange
        var animals = new List<Animal>
        {
            new DumbAnimal("Test1", 0, 0, 0),
            new DumbAnimal("Test2", 0, 0, 0),
            new DumbAnimal("Test3", 0, 0, 0),
        };
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        animalsRepository.Get().Returns(animals);
        var vetClinic = Substitute.For<IVetClinic>();
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        var result = zoo.Animals;
        
        // Assert
        Assert.Equal(animals, result);
    }
    
    [Fact]
    public void Things_Call_ShouldReturnExpectedValue()
    {
        // Arrange
        var things = new List<Thing>
        {
            new DumbThing(0),
            new DumbThing(0),
            new DumbThing(0),
        };
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        thingsRepository.Get().Returns(things);
        var vetClinic = Substitute.For<IVetClinic>();
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        var result = zoo.Things;
        
        // Assert
        Assert.Equal(things, result);
    }
    
    [Fact]
    public void GetPettingAnimals_Call_ShouldReturnExpectedValue()
    {
        // Arrange
        var animals = new List<Animal>
        {
            new DumbAnimal("NotKind1",0, 0, 0),
            new DumbAnimal("NotKind2", 0, 0, 0),
            new DumbKindAnimal("NotKind3", 0, 0, 0, 0),
            new DumbKindAnimal("Test1", 0, 0, 0, 6),
            new DumbKindAnimal("Test2", 0, 0, 0, 10),
            new DumbKindAnimal("Test3", 0, 0, 0, 10)
        };
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        animalsRepository.Get().Returns(animals);
        var vetClinic = Substitute.For<IVetClinic>();
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        var result = zoo.GetPettingAnimals();
        
        // Assert
        Assert.Equal(animals.Slice(3, 3), result);
    }
    
    [Fact]
    public void TotalAnimals_Call_ShouldReturnExpectedValue()
    {
        // Arrange
        var animals = new List<Animal>
        {
            new DumbAnimal("Test1", 0, 0, 0),
            new DumbAnimal("Test2", 0, 0, 0),
            new DumbAnimal("Test3", 0, 0, 0),
        };
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        animalsRepository.Get().Returns(animals);
        var vetClinic = Substitute.For<IVetClinic>();
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        var result = zoo.TotalAnimals;
        
        // Assert
        Assert.Equal(3, result);
    }
    
    [Fact]
    public void TotalThings_Call_ShouldReturnExpectedValue()
    {
        // Arrange
        var things = new List<Thing>
        {
            new DumbThing(0),
            new DumbThing(0),
            new DumbThing(0),
        };
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        thingsRepository.Get().Returns(things);
        var vetClinic = Substitute.For<IVetClinic>();
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        var result = zoo.TotalThings;
        
        // Assert
        Assert.Equal(3, result);
    }
    
    [Fact]
    public void GetTotalFood_Call_ShouldReturnExpectedValue()
    {
        // Arrange
        var animals = new List<Animal>
        {
            new DumbAnimal("Test1", 1, 0, 0),
            new DumbAnimal("Test2", 2, 0, 0),
            new DumbAnimal("Test3", 3, 0, 0),
        };
        var thingsRepository = Substitute.For<IThingsRepository>();
        var animalsRepository = Substitute.For<IAnimalsRepository>();
        animalsRepository.Get().Returns(animals);
        var vetClinic = Substitute.For<IVetClinic>();
        
        var zoo = new Zoo(animalsRepository, thingsRepository, vetClinic);
        
        // Act
        var result = zoo.GetTotalFood();
        
        // Assert
        Assert.Equal(6, result);
    }
    
}