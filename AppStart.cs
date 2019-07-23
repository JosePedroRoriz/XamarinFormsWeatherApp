using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;
using XamarinFormsWeatherApp.Weather.ViewModels;

namespace XamarinFormsWeatherApp
{
    public class AppStart : MvxAppStart
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public AppStart(IMvxApplication app,
            IMvxNavigationService mvxNavigationService)
            : base(app, mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;

            AppCenter.Start("app store key",
               typeof(Analytics), typeof(Crashes));
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return _mvxNavigationService.Navigate<WeatherContainer>();
        }
    }
}
