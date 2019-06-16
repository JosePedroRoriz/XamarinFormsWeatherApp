using MvvmCross.ViewModels;
using System;
using WeatherWebservices.OpenWeatherModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFormsWeatherApp.Weather.Models;

namespace XamarinFormsWeatherApp.Weather.ViewModels
{
    public class WeatherPageViewModel : WeatherViewModel
    {
        #region Variables

        private DateTime _date;
        private Main _weatherMain;

        private MvxObservableCollection<WeatherWebservices.OpenWeatherModels.Weather> _weatherCollection;
        private MvxObservableCollection<Clouds> _cloudsCollection;
        private MvxObservableCollection<TemperatureInformation> _temperatureInformationCollection;
        private MvxObservableCollection<WindInformation> _windInformationCollection;
        private string _currentUvIndex;
        private string _currentHumidity;
        private string _curentPressure;
        private string _sunsetSunrise;
        private string _currentWeatherDescription;
        private MvxObservableCollection<SysInformation> _systemInformationCollection;

        #endregion

        #region Constructor
        public WeatherPageViewModel()
        {

        }

        public WeatherPageViewModel(List reportList, DateTime date)
        {
            Date = date;

            WeatherCollection = new MvxObservableCollection<WeatherWebservices.OpenWeatherModels.Weather> { reportList.weather[0] };
            IsDataValid = true;
            CloudsCollection = new MvxObservableCollection<Clouds> { reportList.clouds };

            TemperatureInformationCollection = new MvxObservableCollection<TemperatureInformation> { new TemperatureInformation(date, reportList.main) };
            WindInformationCollection = new MvxObservableCollection<WindInformation> { new WindInformation(date, reportList.wind) };

            //waiting on a api update so i can use this...
            //SystemInformationCollection = new MvxObservableCollection<SysInformation> { new SysInformation(date, reportList.sys) };
        }

        #endregion

        #region Properties

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        public MvxObservableCollection<WeatherWebservices.OpenWeatherModels.Weather> WeatherCollection
        {
            get => _weatherCollection;
            set
            {
                _weatherCollection = value;
                RaisePropertyChanged(() => WeatherCollection);
            }
        }

        public Main WeatherMain
        {
            get => _weatherMain;
            set
            {
                _weatherMain = value;
                RaisePropertyChanged(() => WeatherMain);
            }
        }

        public MvxObservableCollection<TemperatureInformation> TemperatureInformationCollection
        {
            get => _temperatureInformationCollection;
            set
            {
                _temperatureInformationCollection = value;
                RaisePropertyChanged(() => TemperatureInformationCollection);
            }
        }

        public MvxObservableCollection<WindInformation> WindInformationCollection
        {
            get => _windInformationCollection;
            set
            {
                _windInformationCollection = value;
                RaisePropertyChanged(() => WindInformationCollection);
            }
        }

        public MvxObservableCollection<Clouds> CloudsCollection
        {
            get => _cloudsCollection;
            set
            {
                _cloudsCollection = value;
                RaisePropertyChanged(() => CloudsCollection);
            }
        }

        public MvxObservableCollection<SysInformation> SystemInformationCollection
        {
            get => _systemInformationCollection;
            set
            {
                _systemInformationCollection = value;
                RaisePropertyChanged(() => SystemInformationCollection);
            }
        }

        public string CurrentUvIndex
        {
            get => _currentUvIndex;
            set
            {
                _currentUvIndex = value;
                RaisePropertyChanged(() => CurrentUvIndex);
            }
        }

        public string CurrentHumidity
        {
            get => _currentHumidity;
            set
            {
                _currentHumidity = value;
                RaisePropertyChanged(() => CurrentHumidity);
            }
        }

        public string CurentPressure
        {
            get => _curentPressure;
            set
            {
                _curentPressure = value;
                RaisePropertyChanged(() => CurentPressure);
            }
        }

        public string CurrentWeatherDescription
        {
            get => _currentWeatherDescription;
            set
            {
                _currentWeatherDescription = value;
                RaisePropertyChanged(() => CurrentWeatherDescription);
            }
        }

        //waiting on the api to add this to the week prediction and not do several calls for this, it comes in xml but not json for the same request
        public string SunsetSunrise
        {
            get => _sunsetSunrise;
            set
            {
                _sunsetSunrise = value;
                RaisePropertyChanged(() => SunsetSunrise);
            }
        }

        #endregion
    }
}
