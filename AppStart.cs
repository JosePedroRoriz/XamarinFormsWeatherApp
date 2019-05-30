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

            AppCenter.Start("android=587116ff-fe34-430f-a2d6-2fac8dcf1f20;",
               typeof(Analytics), typeof(Crashes));
        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return _mvxNavigationService.Navigate<WeatherContainer>();
        }
    }
}
