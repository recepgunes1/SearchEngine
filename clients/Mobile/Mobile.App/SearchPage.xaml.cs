using Mobile.Infrastructure.Services.Abstracts;

namespace Mobile.App;

public partial class SearchPage : ContentPage
{
    private readonly IHttpClientService _httpClientService;

    public SearchPage(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
        InitializeComponent();
    }

    private async void InputBar_OnSearchButtonPressed(object sender, EventArgs e)
    {
        var input = InputBar.Text.Trim();
        if (!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
            await Navigation.PushAsync(new SearchResultPage(_httpClientService, InputBar.Text, 1, 10));
    }

    private async void LuckyButton_OnClicked(object sender, EventArgs e)
    {
        var address = await _httpClientService.GetLuckyUrl(InputBar.Text);

        if (!string.IsNullOrEmpty(address) && !string.IsNullOrWhiteSpace(address))
        {
            await Navigation.PushAsync(new ResultWebPage(address));
            return;
        }

        var answer = await DisplayAlert("Information", $"There is no page for {InputBar.Text}", "Accept", "Cancel");
        if (answer) InputBar.Text = string.Empty;
    }

    private async void InputBar_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var results = await _httpClientService.GetSuggestions(InputBar.Text);
        if (results.Count <= 0) return;

        ListViewSuggestions.IsVisible = true;
        ListViewSuggestions.ItemsSource = results;
    }
}