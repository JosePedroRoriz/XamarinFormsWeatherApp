using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using System.Globalization;
using Xamarin.Forms;
using XamarinFormsWeatherApp.Localizations;

namespace XamarinFormsWeatherApp
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                WeatherMainPageLocalization.Culture = ci;
                DependencyService.Get<ILocalize>().SetLocale(ci);
            }

            //uncomment to test pt strings
            //WeatherMainPageLocalization.Culture = new CultureInfo("pt-PT");

            CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton();

            // Construct custom application start object
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, AppStart>();

            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);

            // request a reference to the constructed appstart object 
            var appStart = Mvx.IoCProvider.Resolve<IMvxAppStart>();

            // register the appstart object
            RegisterAppStart(appStart);
        }
    }
}
