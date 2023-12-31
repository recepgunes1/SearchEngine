using System.Text;
using Microsoft.Extensions.Configuration;
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
            var jsonData = $"{{\"Data\":\"{Convert.ToBase64String(bytes)}\"}}";
            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync($"{api}/summarize",
                new StringContent(jsonData, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return string.Empty;
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
        catch
        {
            return string.Empty;
        }
    }
}