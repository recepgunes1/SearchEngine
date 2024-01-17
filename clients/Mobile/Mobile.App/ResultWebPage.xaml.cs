namespace Mobile.App;

public partial class ResultWebPage : ContentPage
{
    public ResultWebPage(string source)
    {
        InitializeComponent();
        WebViewResult.Source = source;
    }
}