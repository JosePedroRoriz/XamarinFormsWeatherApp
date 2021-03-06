using MvvmCross.Forms.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormsWeatherApp.Weather.ViewModels;

namespace XamarinFormsWeatherApp.Weather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeatherPage : MvxContentPage<WeatherPageViewModel>
    {
        public WeatherPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<WeatherViewModel>(this, "RefreshFinished", (viewModel) => { MainThread.BeginInvokeOnMainThread(() => { PullToRefreshLayout.IsRefreshing = false; }); });
        }
    }
}
