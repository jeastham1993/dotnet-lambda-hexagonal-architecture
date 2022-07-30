using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CrudSample.Core.Command;
using CrudSample.Core.Queries;
using HelloWorld;
using Microsoft.Extensions.DependencyInjection;
using CrudSample.Core.Services;
using Amazon.XRay.Recorder.Core;

namespace CreateProduct
{
    public class Function
    {
        private readonly CreateProductCommandHandler _handler;
        private readonly ILoggingService _loggingService;

        public Function() : this(null)
        {
        }

        internal Function(CreateProductCommandHandler handler = null, ILoggingService loggingService = null)
        {
            Startup.ConfigureServices();

            this._handler = handler ?? Startup.Services.GetRequiredService<CreateProductCommandHandler>();
            this._loggingService = loggingService ?? Startup.Services.GetRequiredService<ILoggingService>();
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
            this._loggingService.AddTraceId(AWSXRayRecorder.Instance.GetEntity().TraceId);

            if (apigProxyEvent.HttpMethod != "POST" || string.IsNullOrEmpty(apigProxyEvent.Body))
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 400,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
                };
            }

            var product =
                await this._handler.Handle(JsonSerializer.Deserialize<CreateProductCommand>(apigProxyEvent.Body));

            return new APIGatewayProxyResponse
            {
                Body = JsonSerializer.Serialize(product),
                StatusCode = 200,
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };
        }
    }
}
