using HSEBank.Infrastructure.Export;
using HSEBank.Infrastructure.Repositories.Cached;
using HSEBank.Infrastructure.Repositories.InMemory;
using HSEBank.UseCases.Analytics;
using HSEBank.UseCases.DataSources;
using HSEBank.UseCases.Export;
using HSEBank.UseCases.Facades;
using HSEBank.UseCases.Factories;
using HSEBank.UseCases.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace HSEBank.CLI;

public static class CompositionRoot
{
    private static IServiceProvider? _services;

    public static IServiceProvider Services => _services ??= CreateServiceProvider();

    private static IServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        
        services.AddMemoryCache();

        services.AddSingleton<InMemoryAccountsRepository>();
        services.AddSingleton<InMemoryCategoriesRepository>();
        services.AddSingleton<InMemoryOperationsRepository>();

        services.AddSingleton<IAccountsRepository>(provider =>
            new CachedAccountsRepository(provider.GetRequiredService<InMemoryAccountsRepository>(), provider.GetRequiredService<IMemoryCache>()));
        services.AddSingleton<ICategoriesRepository>(provider =>
            new CachedCategoriesRepository(provider.GetRequiredService<InMemoryCategoriesRepository>(), provider.GetRequiredService<IMemoryCache>()));
        services.AddSingleton<IOperationsRepository>(provider =>
            new CachedOperationsRepository(provider.GetRequiredService<InMemoryOperationsRepository>(),
                provider.GetRequiredService<IMemoryCache>()));

        services.AddSingleton<ICoreEntitiesCreator, CoreEntitiesFactory>();
        
        services.AddSingleton<IBankAccountFacade, BankAccountFacade>();
        services.AddSingleton<ICategoryFacade, CategoryFacade>();
        services.AddSingleton<IOperationFacade, OperationFacade>();

        services.AddSingleton<IBankAccountsGetter>(provider => provider.GetRequiredService<IBankAccountFacade>());
        services.AddSingleton<ICategoriesGetter>(provider => provider.GetRequiredService<ICategoryFacade>());
        services.AddSingleton<IOperationsGetter>(provider => provider.GetRequiredService<IOperationFacade>());

        services.AddSingleton<IAnalyticsService, AnalyticsService>();
        
        services.AddSingleton<ICoreEntitiesAggregator, CoreEntitiesAggregator>();
        
        services.AddTransient<JsonExporter>();

        services.AddTransient<YamlExporter>();
        
        return services.BuildServiceProvider();
    }
    
    public static IBankAccountFacade BankAccountFacade => Services.GetRequiredService<IBankAccountFacade>();
    
    public static ICategoryFacade CategoryFacade => Services.GetRequiredService<ICategoryFacade>();
    
    public static IOperationFacade OperationFacade => Services.GetRequiredService<IOperationFacade>();
    
    public static IAnalyticsService AnalyticsService => Services.GetRequiredService<IAnalyticsService>();
    
    public static JsonExporter JsonExporter => Services.GetRequiredService<JsonExporter>();
    
    public static YamlExporter YamlExporter => Services.GetRequiredService<YamlExporter>();
}