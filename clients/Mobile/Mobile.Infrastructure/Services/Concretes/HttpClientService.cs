using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Mobile.Infrastructure.Models;
using Mobile.Infrastructure.Services.Abstracts;

namespace Mobile.Infrastructure.Services.Concretes;

public class HttpClientService : IHttpClientService
{
    private readonly IConfiguration _configuration;

    public HttpClientService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<ResultModel>> GetResults(string input, int page, int pageSize)
    {
        try
        {
            var gatewayApiUrl = _configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();

            using var client = new HttpClient();
            var response =
                await client.GetAsync($"{gatewayApiUrl}/search?input={input}&page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode) return new List<ResultModel>();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true
            };

            return JsonSerializer.Deserialize<List<ResultModel>>(jsonResponse, options) ?? new List<ResultModel>();
        }
        catch
        {
            return new();
        }
    }

    public async Task<ObservableCollection<SuggestionModel>> GetSuggestions(string input)
    {
        try
        {
            var gatewayApiUrl = _configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();

            using var client = new HttpClient();

            var response = await client.GetAsync($"{gatewayApiUrl}/suggest?input={input}");

            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return new ();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(jsonResponse) && !string.IsNullOrWhiteSpace(jsonResponse))
                return JsonSerializer.Deserialize<ObservableCollection<SuggestionModel>>(jsonResponse) ?? new();

            return new();
        }
        catch
        {
            return new();
        }
    }

    public async Task<string> GetLuckyUrl(string input)
    {
        try
        {
            var gatewayApiUrl = _configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();
            using var client = new HttpClient();
            var response = await client.GetAsync($"{gatewayApiUrl}/lucky?input={input}");
            response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode) return string.Empty;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonResponse) && !string.IsNullOrWhiteSpace(jsonResponse)) return jsonResponse;
            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    public async Task<string> Register(string input)
    {
        try
        {
            var gatewayApiUrl = _configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();

            var urlDto = new RegisterUrlDto
            {
                Link = input
            };
            var jsonContent = JsonSerializer.Serialize(urlDto);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            var response = await client.PostAsync($"{gatewayApiUrl}/register", content);

            var result = await response.Content.ReadAsStringAsync();

            return result;
        }
        catch(Exception ex)
        {
            return ex.Message;
        }
    }
}