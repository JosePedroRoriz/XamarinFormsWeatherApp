using MvvmCross.Forms.Views;
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
        }
    }
}