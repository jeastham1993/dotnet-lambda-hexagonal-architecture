using Amazon.DynamoDBv2;
using CrudSample.Core.Command;
using CrudSample.Core.Models;
using CrudSample.Core.Queries;
using CrudSample.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HelloWorld;

public static class Startup
{
    public static ServiceProvider Services { get; private set; }

    public static void ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(new AmazonDynamoDBClient());
        serviceCollection.AddTransient<IProductRepository, DynamoDbProductRepository>();
        serviceCollection.AddSingleton<GetProductQueryHandler>();
        serviceCollection.AddSingleton<CreateProductCommandHandler>();
        
        Services = serviceCollection.BuildServiceProvider();
    }
}
