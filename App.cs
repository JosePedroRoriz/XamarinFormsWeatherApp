using Acr.UserDialogs;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace XamarinFormsWeatherApp
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
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
