using Acr.UserDialogs;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherWebservices.OpenWeatherModels;
using WebServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinFormsWeatherApp.Weather.ViewModels
{
    public class WeatherContainer : MvxViewModel
    {
        #region Variables

        private readonly WeatherWebServiceProvider _weatherWebServiceProvider;
        private WeatherPageViewModel _forecast;
        private WeatherPageViewModel _tomorrowForecast;
        private MvxObservableCollection<WeatherPageViewModel> _current5DayForecast;
        private bool _isCelsiusSelected;
        private string _currentTempMode;
        private string _countryName;
        private string _countryCode;
        private string _cityName;
        private bool _isRefreshing;

        private const string Fahrenheit = "Fº";
        private const string Celsius = "Cº";

        #endregion

        #region Constructor

        public WeatherContainer()
        {
            IsCelsiusSelected = true;
            CurrentTempMode = Celsius;

            _weatherWebServiceProvider = WeatherWebServiceProvider.Instance;
        }

        #endregion

        #region Properties 

        public WeatherPageViewModel Forecast
        {
            get => _forecast;
            set
            {
                _forecast = value;
                RaisePropertyChanged(() => Forecast);
            }
        }

        public WeatherPageViewModel TomorrowForecast
        {
            get => _tomorrowForecast;
            set
            {
                _tomorrowForecast = value;
                RaisePropertyChanged(() => TomorrowForecast);
            }
        }

        //for the collectionView view
        public MvxObservableCollection<WeatherPageViewModel> Current5DayForecast
        {
            get => _current5DayForecast;
            set
            {
                _current5DayForecast = value;
                RaisePropertyChanged(() => Current5DayForecast);
            }
        }

        //no need to add kelvin to the mix
        public bool IsCelsiusSelected
        {
            get => _isCelsiusSelected;
            set
            {
                _isCelsiusSelected = value;
                RaisePropertyChanged(() => IsCelsiusSelected);
            }
        }

        public string CurrentTempMode
        {
            get => _currentTempMode;
            set
            {
                _currentTempMode = value;
                RaisePropertyChanged(() => CurrentTempMode);
            }
        }

        public string CountryName
        {
            get => _countryName;
            set
            {
                _countryName = value;
                RaisePropertyChanged(() => CountryName);
            }
        }

        public string CountryCode
        {
            get => _countryCode;
            set
            {
                _countryCode = value;
                RaisePropertyChanged(() => CountryCode);
            }
        }

        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = value;
                RaisePropertyChanged(() => CityName);
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged(nameof(IsRefreshing));
            }
        }

        #endregion

        #region Command

        private IMvxAsyncCommand _toggleSwitchCommand;

        public IMvxAsyncCommand ToggleSwitchCommand
        {
            get
            {
                _toggleSwitchCommand = _toggleSwitchCommand ?? new MvxAsyncCommand(ToggleSwitch);
                return _toggleSwitchCommand;
            }
        }

        private async Task ToggleSwitch()
        {
            await Task.Run(() =>
            {
                //change to fahrenheit 
                if (IsCelsiusSelected)
                {
                    ToFahrenheit();
                    CurrentTempMode = Fahrenheit;
                }

                else
                {
                    ToCelsius();
                    CurrentTempMode = Celsius;
                }

                IsCelsiusSelected = !IsCelsiusSelected;
            });
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    await RefreshData();

                    IsRefreshing = false;
                });
            }
        }

        private Task RefreshData()
        {
            return null;
        }

        //When the user presses tomorrow, fill the next viewModel

        #endregion

        #region Private Methods 

        //todo: change to receive the current selected viewModel
        private void ToFahrenheit()
        {
            Forecast.WeatherMain.temp = ConvertTempToFahrenheit(Forecast.WeatherMain.temp);
            Forecast.WeatherMain.temp_max = ConvertTempToFahrenheit(Forecast.WeatherMain.temp_max);
            Forecast.WeatherMain.temp_min = ConvertTempToFahrenheit(Forecast.WeatherMain.temp_min);
        }

        private double ConvertTempToFahrenheit(double temp) => (temp * 9 / 5) + 32;

        private void ToCelsius()
        {
            Forecast.WeatherMain.temp = ConvertTempToCelsius(Forecast.WeatherMain.temp);
            Forecast.WeatherMain.temp_max = ConvertTempToCelsius(Forecast.WeatherMain.temp_max);
            Forecast.WeatherMain.temp_min = ConvertTempToCelsius(Forecast.WeatherMain.temp_min);
        }

        private double ConvertTempToCelsius(double temp) => (temp - 32) * 5 / 9;

        #endregion

        #region Events

        public override async Task Initialize()
        {
            try
            {
                //get the current location coordinates
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);

                using (UserDialogs.Instance.Loading("Loading..."))
                {
                    //something can go wrong and if it does, it needs to be logged!
                    try
                    {
                        Location location = await Geolocation.GetLocationAsync(new GeolocationRequest() { DesiredAccuracy = GeolocationAccuracy.Medium, Timeout = TimeSpan.FromSeconds(2) });

                        //this causes erros for now
                        //get the city & country
                        //var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                        //var placemark = placemarks?.FirstOrDefault();

                        //if (placemark != null)
                        //{
                        //    CountryName = placemark.CountryName;
                        //    CountryCode = placemark.CountryCode;
                        //    CityName = placemark.FeatureName;
                        //}

                        await GetWeatherReport(request, false, location);
                    }

                    catch (Exception ex)
                    {
                        Analytics.TrackEvent("Something went wrong with the api call");
                        Crashes.TrackError(ex);
                    }
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                Analytics.TrackEvent("FeatureNotSupportedException ");
                Crashes.TrackError(fnsEx);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Analytics.TrackEvent("Handle not enabled on device exception ");
                Crashes.TrackError(fneEx);
            }
            catch (PermissionException pEx)
            {
                Mvx.IoCProvider.Resolve<IUserDialogs>().Alert("Permission to use the currrent location needs to be granted, no data will be loaded");
                Analytics.TrackEvent("Handle permission exception");
                Crashes.TrackError(pEx);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Unable to get location ");
                Crashes.TrackError(ex);
            }
        }

        private async Task GetWeatherReport(GeolocationRequest request, bool getCurrentDayReport, Location location)
        {
            try
            {
                if (!getCurrentDayReport)
                {
                    RootObject rootObject =
                        await _weatherWebServiceProvider.GetForecastForFiveDays(location.Latitude, location.Longitude,
                            IsCelsiusSelected);
                    List<WeatherPageViewModel> currentForecast = new List<WeatherPageViewModel>();

                    foreach (var forecast in rootObject.list)
                    {
                        //in US format
                        DateTime date = Convert.ToDateTime(forecast.dt_txt, CultureInfo.InvariantCulture);
                        var selectedForecast = currentForecast.FirstOrDefault(x => x.Date.Date == date.Date);

                        if (selectedForecast == null)
                            currentForecast.Add(new WeatherPageViewModel(forecast, date));
                        else
                        {
                            selectedForecast.WindCollection.Add(forecast.wind);
                            selectedForecast.CloudsCollection.Add(forecast.clouds);
                            selectedForecast.DateCollection.Add(date);
                            selectedForecast.WeatherCollection.Add(forecast.weather[0]);
                            selectedForecast.WeatherMainCollection.Add(forecast.main);
                        }
                    }
                }

                else
                {
                    RootObject rootObject =
                        await _weatherWebServiceProvider.GetCurrentForecast(location.Latitude, location.Longitude,
                            IsCelsiusSelected);
                    Forecast = new WeatherPageViewModel(rootObject);
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Error getting weather report ");
                Crashes.TrackError(ex);
            }
        }

        #endregion
    }
}
