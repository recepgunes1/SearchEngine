using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace MobileApp;

public partial class RegisterPage : ContentPage
{
    private readonly IConfiguration _configuration;

    public RegisterPage(IConfiguration configuration)
    {
        _configuration = configuration;
        InitializeComponent();
    }

    private async void ButtonRegister_OnClicked(object sender, EventArgs e)
    {
        var gatewayApiUrl = _configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();

        var urlDto = new RegisterUrlDto
        {
            Link = EntryUrl.Text
        };
        var jsonContent = JsonSerializer.Serialize(urlDto);

        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        using var client = new HttpClient();

        var response = await client.PostAsync($"{gatewayApiUrl}/register", content);

        var result = await response.Content.ReadAsStringAsync();

        var answer = await DisplayAlert("Information", result, "Accept", "Cancel");

        if (answer) EntryUrl.Text = string.Empty;
    }


    private class RegisterUrlDto
    {
        public string Link { get; set; } = null!;
    }
}