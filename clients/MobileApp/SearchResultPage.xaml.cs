using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MobileApp.Models;

namespace MobileApp;

public partial class SearchResultPage : ContentPage
{
    private readonly IConfiguration _configuration;
    private readonly string _input;
    private readonly int _page;
    private readonly int _pageSize;

    public SearchResultPage(IConfiguration configuration, string input, int page, int pageSize)
    {
        _configuration = configuration;
        _input = input;
        _page = page;
        _pageSize = pageSize;
        InitializeComponent();
        LoadResults();
        ButtonPrevious.IsEnabled = _page - 1 != 0;
    }


    private async void LoadResults()
    {
        var gatewayApiUrl = _configuration.GetConnectionString("Gateway") ?? throw new ArgumentNullException();

        using var client = new HttpClient();
        var response =
            await client.GetAsync($"{gatewayApiUrl}/search?input={_input}&page={_page}&pageSize={_pageSize}");
        response.EnsureSuccessStatusCode();
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                AllowTrailingCommas = true
            };
            ListViewSearchResult.ItemsSource =
                JsonSerializer.Deserialize<ObservableCollection<ResultModel>>(jsonResponse, options) ??
                new ObservableCollection<ResultModel>();
        }
    }

    private async void ButtonPrevious_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchResultPage(_configuration, _input, _page - 1, 10));
    }

    private async void ButtonNext_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchResultPage(_configuration, _input, _page + 1, 10));
    }

    private async void ListViewSearchResult_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var item = (ResultModel)e.SelectedItem;
        await Navigation.PushAsync(new ResultWebPage(item.Link));
    }
}