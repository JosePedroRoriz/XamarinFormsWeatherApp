using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WeatherWebservices.OpenWeatherModels;
using XamarinFormsWeatherApp.Weather.Interfaces;
using XamarinFormsWeatherApp.Weather.Models;

namespace XamarinFormsWeatherApp.Weather.ViewModels
{
    public class WeatherPageViewModel : WeatherViewModel
    {
        #region Variables

        private DateTime _date;
        private MvxObservableCollection<Clouds> _cloudsCollection;
        private ObservableCollection<ITemperatureInformation> _temperatureInformationCollection;
        private MvxObservableCollection<IWindInformation> _windInformationCollection;
        private string _currentUvIndex;
        private string _currentHumidity;
        private string _curentPressure;
        private string _sunsetSunrise;
        private string _currentWeatherDescription;
        private MvxObservableCollection<ISysInformation> _systemInformationCollection;

        #endregion

        #region Constructor
        public WeatherPageViewModel()
        {

        }

        public WeatherPageViewModel(string description, DateTime date, IWindInformation windInformation, ITemperatureInformation temperature)
        {
            IsDataValid = true;
            Date = date;

            TemperatureInformationCollection = new MvxObservableCollection<ITemperatureInformation>() { temperature };
            WindInformationCollection = new MvxObservableCollection<IWindInformation> { windInformation };
            SystemInformationCollection = new MvxObservableCollection<ISysInformation>();
            CurrentWeatherDescription = description;
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

        public ObservableCollection<ITemperatureInformation> TemperatureInformationCollection
        {
            get => _temperatureInformationCollection;
            set
            {
                _temperatureInformationCollection = value;
                RaisePropertyChanged(() => TemperatureInformationCollection);
            }
        }

        public MvxObservableCollection<IWindInformation> WindInformationCollection
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

        public MvxObservableCollection<ISysInformation> SystemInformationCollection
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
