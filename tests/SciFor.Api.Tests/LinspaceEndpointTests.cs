using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SciFor.Api.Tests;

public sealed class LinspaceEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public LinspaceEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task S1_S2_linspace_defaults_200()
    {
        // @scenario S1 @scenario S2 — happy path and omitted optionals use Fortran defaults (no mesh)
        var response = await _client.PostAsJsonAsync("/v1/linspace", new { start = 0.0, stop = 1.0, num = 5 });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<LinspaceResponse>(JsonOptions);
        Assert.NotNull(body);
        Assert.Equal(5, body.Values.Length);
        Assert.Null(body.Mesh);
        Assert.Equal(0.0, body.Values[0]);
        Assert.Equal(0.25, body.Values[1]);
        Assert.Equal(0.5, body.Values[2]);
        Assert.Equal(0.75, body.Values[3]);
        Assert.Equal(1.0, body.Values[4]);
    }

    [Fact]
    public async Task S9_S11_linspace_400()
    {
        var negativeNum = await _client.PostAsJsonAsync("/v1/linspace", new { start = 0.0, stop = 1.0, num = -1 });
        Assert.Equal(HttpStatusCode.BadRequest, negativeNum.StatusCode);
        Assert.Equal("application/problem+json", negativeNum.Content.Headers.ContentType?.MediaType);

        var bothEndpointsNum1 = await _client.PostAsJsonAsync("/v1/linspace", new { start = 0.0, stop = 1.0, num = 1 });
        Assert.Equal(HttpStatusCode.BadRequest, bothEndpointsNum1.StatusCode);

        // DEF-001: num=0 with endpoint excluded
        var def001 = await _client.PostAsJsonAsync(
            "/v1/linspace",
            new { start = 0.0, stop = 1.0, num = 0, istart = true, iend = false });
        Assert.Equal(HttpStatusCode.BadRequest, def001.StatusCode);
        Assert.Equal("application/problem+json", def001.Content.Headers.ContentType?.MediaType);

        // ADR-002: missing required fields → 400 (not CLR default)
        var missingNum = await _client.PostAsync(
            "/v1/linspace",
            new StringContent("""{"start":0,"stop":1}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, missingNum.StatusCode);
        Assert.Equal("application/problem+json", missingNum.Content.Headers.ContentType?.MediaType);
    }

    private sealed class LinspaceResponse
    {
        public double[] Values { get; set; } = Array.Empty<double>();
        public double? Mesh { get; set; }
    }
}
