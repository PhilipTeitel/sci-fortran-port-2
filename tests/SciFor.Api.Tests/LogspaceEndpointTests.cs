using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SciFor.Api.Tests;

public sealed class LogspaceEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public LogspaceEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task S12_S13_logspace_defaults_200()
    {
        var response = await _client.PostAsJsonAsync("/v1/logspace", new { start = 1.0, stop = 1000.0, num = 5 });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ValuesResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(5, body.Values.Length);
        Assert.Equal(1.0, body.Values[0], precision: 12);
        Assert.Equal(1000.0, body.Values[4], precision: 9);
    }

    [Fact]
    public async Task S15_S18_logspace_400()
    {
        var negative = await _client.PostAsJsonAsync("/v1/logspace", new { start = -1.0, stop = 1000.0, num = 5 });
        Assert.Equal(HttpStatusCode.BadRequest, negative.StatusCode);
        Assert.Equal("application/problem+json", negative.Content.Headers.ContentType?.MediaType);

        var badBase = await _client.PostAsJsonAsync("/v1/logspace", new { start = 1.0, stop = 1000.0, num = 5, @base = 1.0 });
        Assert.Equal(HttpStatusCode.BadRequest, badBase.StatusCode);

        var numNeg = await _client.PostAsJsonAsync("/v1/logspace", new { start = 1.0, stop = 1000.0, num = -2 });
        Assert.Equal(HttpStatusCode.BadRequest, numNeg.StatusCode);

        var num1 = await _client.PostAsJsonAsync("/v1/logspace", new { start = 1.0, stop = 1000.0, num = 1 });
        Assert.Equal(HttpStatusCode.BadRequest, num1.StatusCode);

        var missingNum = await _client.PostAsync(
            "/v1/logspace",
            new StringContent("""{"start":1,"stop":1000}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, missingNum.StatusCode);
        Assert.Equal("application/problem+json", missingNum.Content.Headers.ContentType?.MediaType);
    }

    private sealed class ValuesResponse
    {
        public double[] Values { get; set; } = Array.Empty<double>();
    }
}
