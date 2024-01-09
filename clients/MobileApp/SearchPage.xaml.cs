using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Devices;

namespace MobileApp;

public partial class SearchPage : ContentPage
{
    private readonly IConfiguration _configuration;

    public SearchPage(IConfiguration configuration)
    {
        _configuration = configuration;
        InitializeComponent();
    }

    private async void InputBar_OnSearchButtonPressed(object sender, EventArgs e)
    {
        var gateway = _configuration.GetConnectionString("Gateway")!;
        var answer = await DisplayAlert("Question?", $"gateway: {gateway}", "Accept", "Cancel");
        Debug.WriteLine("Answer: " + answer);
    }

    private async void LuckyButton_OnClicked(object sender, EventArgs e)
    {
        var gatewayApiUrl = _configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();

        using var client = new HttpClient();
        var response = await client.GetAsync($"{gatewayApiUrl}/lucky?input={InputBar.Text}");
        response.EnsureSuccessStatusCode();
        if (!response.IsSuccessStatusCode) return;
        var jsonResponse = await response.Content.ReadAsStringAsync();
        
        
        if (!string.IsNullOrEmpty(jsonResponse) && !string.IsNullOrWhiteSpace(jsonResponse))
        {
            await Navigation.PushAsync(new ResultWebPage(jsonResponse));
            return;
        }
        
        _ = await DisplayAlert("Information", $"There is no page for {InputBar.Text}", "Accept", "Cancel");
    }
}