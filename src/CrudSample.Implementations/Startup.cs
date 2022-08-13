using Amazon.DynamoDBv2;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using CrudSample.Core.Command;
using CrudSample.Core.Models;
using CrudSample.Core.Queries;
using CrudSample.Core.Services;
using CrudSample.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace HelloWorld;

public static class Startup
{
    public static ServiceProvider Services { get; private set; }

    public static void ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        AWSSDKHandler.RegisterXRayForAllServices();

        serviceCollection.AddSingleton<ILoggingService>(new SerilogLogger());
        serviceCollection.AddSingleton(new AmazonDynamoDBClient(new AmazonDynamoDBConfig()));
        serviceCollection.AddTransient<IProductRepository, DynamoDbProductRepository>();
        serviceCollection.AddSingleton<GetProductQueryHandler>();
        serviceCollection.AddSingleton<CreateProductCommandHandler>();
        
        Services = serviceCollection.BuildServiceProvider();
    }
}
