using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.TestUtilities;
using Amazon.XRay.Recorder.Core;
using CrudSample.Core.Models;
using CrudSample.Core.Queries;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ProductApi.Test
{
    public class FunctionTests
    {
        public FunctionTests()
        {
        }

        [Fact]
        public async Task GetProductHandler_ShouldReturnSuccess()
        {
            var testProduct = Product.Create("Test product", 10);
        
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(p => p.Get(It.IsAny<string>())).ReturnsAsync(testProduct);

            var handler = new GetProductQueryHandler(mockRepo.Object);

            var getProductFunction = new GetProduct.Function(handler);

            var queryResult = await getProductFunction.FunctionHandler(
                JsonConvert.DeserializeObject<APIGatewayProxyRequest>(EventHelper.ValidGetProductRequest),
                new TestLambdaContext());

            queryResult.StatusCode.Should().Be(200);
            queryResult.Body.Should().NotBeNull();
        }
    }
}