using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SyncElastic.Infrastructure.Models;
using SyncElastic.Infrastructure.Services.Abstract;

namespace SyncElastic.Infrastructure.Services.Concrete;

public class SummarizeService(IConfiguration configuration) : ISummarizeService
{
    public async Task<string> SummarizeAsync(string input)
    {
        try
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var api = configuration.GetConnectionString("SummarizerApi");
            var sb = new StringBuilder();
            sb.Append("{\"Data\":\"");
            sb.Append(Convert.ToBase64String(bytes));
            sb.Append("\"}");
            var jsonData = sb.ToString();
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{api}/api/summarize",
                new StringContent(jsonData, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return string.Empty;
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = false,
                AllowTrailingCommas = true
            };
            var resultFromSummarizeApi = JsonSerializer.Deserialize<ResultFromSummarizeApi>(jsonResponse, options) ??
                                         new ResultFromSummarizeApi();

            return resultFromSummarizeApi.Summary;
        }
        catch
        {
            return string.Empty;
        }
    }
}