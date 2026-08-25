using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SciFor.Api.Tests;

public sealed class DerivEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public DerivEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task S19_deriv_happy_same_length()
    {
        var response = await _client.PostAsJsonAsync("/v1/deriv", new { f = new[] { 0.0, 1.0, 4.0, 9.0 }, dh = 1.0 });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<DerivResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(4, body.Df.Length);
    }

    [Fact]
    public async Task S21_S22_deriv_400()
    {
        var shortF = await _client.PostAsJsonAsync("/v1/deriv", new { f = new[] { 1.0 }, dh = 0.1 });
        Assert.Equal(HttpStatusCode.BadRequest, shortF.StatusCode);
        Assert.Equal("application/problem+json", shortF.Content.Headers.ContentType?.MediaType);

        var zeroDh = await _client.PostAsJsonAsync("/v1/deriv", new { f = new[] { 1.0, 2.0 }, dh = 0.0 });
        Assert.Equal(HttpStatusCode.BadRequest, zeroDh.StatusCode);
        Assert.Equal("application/problem+json", zeroDh.Content.Headers.ContentType?.MediaType);

        var missingDh = await _client.PostAsync(
            "/v1/deriv",
            new StringContent("""{"f":[1.0,2.0]}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, missingDh.StatusCode);
        Assert.Equal("application/problem+json", missingDh.Content.Headers.ContentType?.MediaType);
    }

    private sealed class DerivResponse
    {
        public double[] Df { get; set; } = Array.Empty<double>();
    }
}
