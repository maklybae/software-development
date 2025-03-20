using HSEBank.CLI;
using HSEBank.UseCases.DataSources;
using HSEBank.UseCases.Facades;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        // var memoryCache = new MemoryCache();
        CompositionRoot.Services.GetRequiredService<ICategoryFacade>().GetAll();
        var consoleApp = new ConsoleApplication(CompositionRoot.CategoryFacade, CompositionRoot.BankAccountFacade,
            CompositionRoot.OperationFacade, CompositionRoot.AnalyticsService);
        consoleApp.Run();
    }
}