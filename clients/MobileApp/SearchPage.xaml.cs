using Microsoft.Extensions.Configuration;

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
        var input = InputBar.Text.Trim();
        if (!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
            await Navigation.PushAsync(new SearchResultPage(_configuration, InputBar.Text, 1, 10));
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

        var answer = await DisplayAlert("Information", $"There is no page for {InputBar.Text}", "Accept", "Cancel");
        if (answer) InputBar.Text = string.Empty;
    }
}