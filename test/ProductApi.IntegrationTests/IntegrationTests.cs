using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace HelloWorld.IntegrationTests;

internal class CreateProductResponse
{
    public string ProductId { get; set; }
}

public class IntegrationTests
{
    private HttpClient _httpClient = new HttpClient();

    private string _apiBase =
        $"{(Environment.GetEnvironmentVariable("API_URL") ?? "")}";

    [Fact]
    public async void GetCallingIp_ShouldReturnOk()
    {
        var createProductBody = new
        {
            Name = "Integration test product",
            Price = 10.00,
            Description = "This is a product created from an integration test"
        };

        var httpResponse = await this._httpClient.PostAsync(_apiBase,
            new StringContent(JsonConvert.SerializeObject(createProductBody), Encoding.UTF8, "application/json"));
        
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);

        var response =
            JsonConvert.DeserializeObject<CreateProductResponse>(await httpResponse.Content.ReadAsStringAsync());

        var getProductResponse = await this._httpClient.GetAsync($"{_apiBase}{response.ProductId}");

        Assert.Equal(HttpStatusCode.OK, getProductResponse.StatusCode);
    }
}