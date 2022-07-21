using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using CrudSample.Core.Command;
using CrudSample.Core.Queries;
using HelloWorld;
using Microsoft.Extensions.DependencyInjection;

namespace CreateProduct
{
    public class Function
    {
        private readonly CreateProductCommandHandler _handler;

        public Function() : this(null)
        {
        }

        internal Function(CreateProductCommandHandler handler = null)
        {
            Startup.ConfigureServices();

            this._handler = handler ?? Startup.Services.GetRequiredService<CreateProductCommandHandler>();
        }

        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest apigProxyEvent, ILambdaContext context)
        {
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
