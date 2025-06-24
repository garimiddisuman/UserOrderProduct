using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace UserService.API.Tests;

public class IntegrationTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task ReturnsHelloWorld()
    {
        var response = await _client.GetAsync("/api/hello");
        var content = await response.Content.ReadAsStringAsync();
        
        Assert.Equal("Hello World!", content);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
