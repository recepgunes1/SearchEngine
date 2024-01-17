using Mobile.Infrastructure.Services.Abstracts;

namespace Mobile.App;

public partial class RegisterPage : ContentPage
{
    private readonly IHttpClientService _httpClientService;

    public RegisterPage(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
        InitializeComponent();
    }

    private async void ButtonRegister_OnClicked(object sender, EventArgs e)
    {
        var result = await _httpClientService.Register(EntryUrl.Text);

        var answer = await DisplayAlert("Information", result, "Accept", "Cancel");

        if (answer) EntryUrl.Text = string.Empty;
    }
}