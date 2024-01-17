using System.Collections.ObjectModel;
using Mobile.Infrastructure.Models;
using Mobile.Infrastructure.Services.Abstracts;

namespace Mobile.App;

public partial class SearchResultPage : ContentPage
{
    private readonly IHttpClientService _httpClientService;
    private readonly string _input;
    private readonly int _page;
    private readonly int _pageSize;

    public SearchResultPage(IHttpClientService httpClientService, string input, int page,
        int pageSize)
    {
        _httpClientService = httpClientService;
        _input = input;
        _page = page;
        _pageSize = pageSize;
        InitializeComponent();
        LoadResults();
        ButtonPrevious.IsEnabled = _page - 1 != 0;
    }


    private async void LoadResults()
    {
        var results = await _httpClientService.GetResults(_input, _page, _pageSize);
        var convertedResult = new ObservableCollection<ResultModel>(results);
        ListViewSearchResult.ItemsSource = convertedResult;
    }

    private async void ButtonPrevious_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchResultPage(_httpClientService, _input, _page - 1, 10));
    }

    private async void ButtonNext_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchResultPage(_httpClientService, _input, _page + 1, 10));
    }

    private async void ListViewSearchResult_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var item = (ResultModel)e.SelectedItem;
        await Navigation.PushAsync(new ResultWebPage(item.Link));
    }
}