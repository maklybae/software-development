using Microsoft.Extensions.DependencyInjection;
using MiniHw1.Interfaces;
using MiniHw1.Models.Animals;
using MiniHw1.Models.Things;
using MiniHw1.Repositories;
using MiniHw1.Services;
using Spectre.Console;
using Table = MiniHw1.Models.Things.Table;

namespace MiniHw1;

public static class Program
{
    private static readonly ServiceProvider Services = ConfigureServices();
    private static readonly Zoo Zoo = Services.GetRequiredService<Zoo>();
    private static int _number = 0;

    private static int Number
    {
        get
        {
            _number++;
            return _number;
        }
    }

    private static ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .AddSingleton<Zoo>()
            .AddSingleton<IVetClinic, VetClinic>()
            .AddSingleton<IAnimalsRepository, ListAnimalsRepository>()
            .AddSingleton<IThingsRepository, ListThingsRepository>()
            .BuildServiceProvider();
    }

    private static class MenuOption
    {
        public const string AddAnimal = "Add Animal";
        public const string AddThing = "Add Thing";
        public const string ShowAllAnimals = "Show All Animals";
        public const string ShowPettingAnimals = "Show Petting Animals";
        public const string ShowFoodConsumption = "Show Food Consumption";
        public const string ShowThings = "Show Things";
        public const string Exit = "Exit";
    }

    private static class AnimalType
    {
        public const string Monkey = "Monkey";
        public const string Rabbit = "Rabbit";
        public const string Tiger = "Tiger";
        public const string Wolf = "Wolf";
    }
    
    private static class ThingType
    {
        public const string Computer = "Computer";
        public const string Table = "Table";
    }

    private static void AddAnimal()
    {
        AnsiConsole.MarkupLine("[underline green]Adding New Animal[/]");

        var name = AnsiConsole.Ask<string>("Name:");
        var type = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Animal Type")
                .PageSize(5)
                .AddChoices(AnimalType.Monkey, AnimalType.Rabbit, AnimalType.Tiger, AnimalType.Wolf));
        AnsiConsole.MarkupLine($"[bold]Animal Type: {type}[/]");

        var foodConsumption = AnsiConsole.Ask<int>("Food consumption:");
        var healthLevel = AnsiConsole.Ask<int>("Health level:");
        
        Animal animal = type switch {
            AnimalType.Monkey => new Monkey(name, foodConsumption, healthLevel, Number),
            AnimalType.Rabbit => new Rabbit(name, foodConsumption, healthLevel, Number),
            AnimalType.Tiger => new Tiger(name, foodConsumption, healthLevel, Number),
            AnimalType.Wolf => new Wolf(name, foodConsumption, healthLevel, Number),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        if (animal is IKind kind)
        {
            kind.Kindness = AnsiConsole.Ask<int>("Kindness:");
        }

        if (Zoo.TryAddAnimal(animal))
        {
            AnsiConsole.MarkupLine($"[green]Animal added successfully[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"[red]Failed to add animal[/]");
        }
    }

    private static void AddThing()
    {
        AnsiConsole.MarkupLine("[underline green]Adding New Thing[/]");
        
        var type = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Thing Type")
                .PageSize(5)
                .AddChoices(ThingType.Computer, ThingType.Table));
        AnsiConsole.MarkupLine($"[bold]Thing Type: {type}[/]");

        Thing thing = type switch
        {
            ThingType.Computer => new Computer(Number),
            ThingType.Table => new Table(Number),
            _ => throw new ArgumentOutOfRangeException()
        };
        
        Zoo.AddThing(thing);
        AnsiConsole.MarkupLine($"[green]Thing added successfully[/]");
    }

    private static void ShowAllAnimals()
    {
        if (Zoo.TotalAnimals == 0)
        {
            AnsiConsole.MarkupLine("[bold]No animals in the zoo[/]");
            return;
        }

        foreach (var animal in Zoo.Animals)
        {
            AnsiConsole.MarkupLine($"[bold]{animal}[/]");
        }
    }

    private static void ShowThings()
    {
        if (Zoo.TotalThings == 0)
        {
            AnsiConsole.MarkupLine("[bold]No things in the zoo[/]");
            return;
        }

        foreach (var thing in Zoo.Things)
        {
            AnsiConsole.MarkupLine($"[bold]{thing}[/]");
        }
    }

    private static void ShowPettingAnimals()
    {
        var pettingAnimals = Zoo.GetPettingAnimals();
        if (!pettingAnimals.Any())
        {
            AnsiConsole.MarkupLine("[bold]No petting animals in the zoo[/]");
            return;
        }

        foreach (var animal in pettingAnimals)
        {
            AnsiConsole.MarkupLine($"[bold]{animal}[/]");
        }
    }

    private static void ShowFoodConsumption()
    {
        AnsiConsole.MarkupLine($"[bold]Total food consumption: {Zoo.GetTotalFood()}[/]");
    }

    private static void RunApplication()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Moscow Zoo")
                    .PageSize(10)
                    .AddChoices(
                        MenuOption.AddAnimal,
                        MenuOption.AddThing,
                        MenuOption.ShowAllAnimals,
                        MenuOption.ShowPettingAnimals,
                        MenuOption.ShowFoodConsumption,
                        MenuOption.ShowThings,
                        MenuOption.Exit));

            switch (choice)
            {
                case MenuOption.AddAnimal:
                    AddAnimal();
                    break;
                case MenuOption.AddThing:
                    AddThing();
                    break;
                case MenuOption.ShowAllAnimals:
                    ShowAllAnimals();
                    break;
                case MenuOption.ShowPettingAnimals:
                    ShowPettingAnimals();
                    break;
                case MenuOption.ShowFoodConsumption:
                    ShowFoodConsumption();
                    break;
                case MenuOption.ShowThings:
                    ShowThings();
                    break;
                case MenuOption.Exit:
                    return;
            }

            AnsiConsole.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private static void Main(string[] args)
    {
        RunApplication();
    }
}