using Acr.UserDialogs;
using Akavache;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;
using System.Globalization;
using Xamarin.Forms;
using XamarinFormsWeatherApp.Localizations;
using XamarinFormsWeatherApp.Weather.Interfaces;
using XamarinFormsWeatherApp.Weather.Models;

namespace XamarinFormsWeatherApp
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                WeatherMainPageLocalization.Culture = ci;
                DependencyService.Get<ILocalize>().SetLocale(ci);
            }

            BlobCache.ApplicationName = "XamarinFormsWeatherApp";

            BlobCache.LocalMachine.InvalidateAll();
            BlobCache.LocalMachine.Vacuum();

            //uncomment to test pt strings
            //WeatherMainPageLocalization.Culture = new CultureInfo("pt-PT");

            CreatableTypes().EndingWith("Service").AsInterfaces().RegisterAsLazySingleton();

            // Construct custom application start object
            Mvx.IoCProvider.ConstructAndRegisterSingleton<IMvxAppStart, AppStart>();

            Mvx.IoCProvider.RegisterSingleton(() => UserDialogs.Instance);

            //weather ioc
            Mvx.IoCProvider.RegisterType<ITemperatureInformation, TemperatureInformation>();
            Mvx.IoCProvider.RegisterType<IForecastInformation, ForecastInformation>();
            Mvx.IoCProvider.RegisterType<IWindInformation, WindInformation>();
            Mvx.IoCProvider.RegisterType<ISysInformation, SysInformation>();

            // request a reference to the constructed appstart object 
            IMvxAppStart appStart = Mvx.IoCProvider.Resolve<IMvxAppStart>();

            // register the appstart object
            RegisterAppStart(appStart);
        }
    }
}
