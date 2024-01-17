using System.Text.Json;

namespace WebApp.Services;

public class HttpClientService(IConfiguration configuration) : IHttpClientService
{
    public async Task<List<string>> GetSuggestions(string input)
    {
        var gatewayApiUrl = configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();

        using var client = new HttpClient();

        var response = await client.GetAsync($"{gatewayApiUrl}/suggest?input={input}");

        response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode) return new List<string>();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (!string.IsNullOrEmpty(jsonResponse) && !string.IsNullOrWhiteSpace(jsonResponse))
            return JsonSerializer.Deserialize<List<string>>(jsonResponse) ?? new List<string>();

        return new List<string>();
    }

    public async Task<string> GetLuckyUrl(string input)
    {
        var gatewayApiUrl = configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();
        using var client = new HttpClient();
        var response = await client.GetAsync($"{gatewayApiUrl}/lucky?input={input}");
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) return string.Empty;
        var jsonResponse = await response.Content.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(jsonResponse) && !string.IsNullOrWhiteSpace(jsonResponse)) return jsonResponse;
        return string.Empty;
    }
}