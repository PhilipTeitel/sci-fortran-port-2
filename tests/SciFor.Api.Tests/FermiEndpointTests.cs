using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SciFor.Api.Tests;

public sealed class FermiEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public FermiEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task S23_S26_fermi()
    {
        // S23 formula at x=0 → 0.5
        var atZero = await Post(0.0, 100.0);
        Assert.Equal(HttpStatusCode.OK, atZero.Status.StatusCode);
        Assert.Equal(0.5, atZero.Body!.Value);

        // S24: x*beta > 100 → exactly 0
        var saturated = await Post(2.0, 100.0);
        Assert.Equal(0.0, saturated.Body!.Value);

        // S25: x*beta == 100 uses formula (not early zero)
        var atCutoff = await Post(1.0, 100.0);
        Assert.NotEqual(0.0, atCutoff.Body!.Value);
        Assert.True(atCutoff.Body.Value > 0.0 && atCutoff.Body.Value < 1e-40);

        // S26: beta=0 → 0.5 (explicit zero is valid; omitted beta must be 400 — see missing_required_fields_400)
        var betaZero = await Post(5.0, 0.0);
        Assert.Equal(0.5, betaZero.Body!.Value);
    }

    [Fact]
    public async Task missing_required_fields_400()
    {
        // ADR-002 / API-1: omitted required x/beta → 400 Problem Details (not CLR default 0).
        var empty = await _client.PostAsJsonAsync("/v1/fermi", new { });
        Assert.Equal(HttpStatusCode.BadRequest, empty.StatusCode);
        Assert.Equal("application/problem+json", empty.Content.Headers.ContentType?.MediaType);

        var missingBeta = await _client.PostAsync(
            "/v1/fermi",
            new StringContent("""{"x":1}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, missingBeta.StatusCode);
        Assert.Equal("application/problem+json", missingBeta.Content.Headers.ContentType?.MediaType);

        var missingX = await _client.PostAsync(
            "/v1/fermi",
            new StringContent("""{"beta":100}""", Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.BadRequest, missingX.StatusCode);
        Assert.Equal("application/problem+json", missingX.Content.Headers.ContentType?.MediaType);
    }

    private async Task<(HttpResponseMessage Status, FermiResponse? Body)> Post(double x, double beta)
    {
        var response = await _client.PostAsJsonAsync("/v1/fermi", new { x, beta });
        var body = response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<FermiResponse>(JsonOptions)
            : null;
        return (response, body);
    }

    private sealed class FermiResponse
    {
        public double Value { get; set; }
    }
}
