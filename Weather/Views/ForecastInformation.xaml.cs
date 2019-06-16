using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormsWeatherApp.Weather.Models;
using XamarinFormsWeatherApp.Weather.ViewModels;

namespace XamarinFormsWeatherApp.Weather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxTabbedPagePresentation(WrapInNavigationPage = false)]
    public partial class ForecastInformation : MvxContentPage<ForecastInformationViewModel>
    {
        public ForecastInformation()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<WeatherViewModel>(this, "RefreshFinished", (viewModel) => { MainThread.BeginInvokeOnMainThread(() => { PullToRefreshLayout.IsRefreshing = false; }); });
        }       
    }
}
