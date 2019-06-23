using Acr.UserDialogs;
using Akavache;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using WeatherWebservices;
using WeatherWebservices.OpenWeatherModels;
using WebServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinFormsWeatherApp.Weather.Interfaces;
using XamarinFormsWeatherApp.Weather.Models;

namespace XamarinFormsWeatherApp.Weather.ViewModels
{
    public class WeatherContainer : MvxViewModel
    {
        #region Variables

        private WeatherPageViewModel _forecast;
        private WeatherPageViewModel _tomorrowForecast;
        private ForecastInformationViewModel _forecastInformation;
        private bool _isCelsiusSelected;
        private string _currentTempMode;
        private string _countryName;
        private string _countryCode;
        private string _cityName;
        private string _selectedCity;
        private bool _isRefreshing;

        private const string Fahrenheit = "Fº";
        private const string Celsius = "Cº";

        private IOpenWeather _openWeather;
        private bool _isFinishedLoading;

        #endregion

        #region Constructor

        public WeatherContainer()
        {
            IsCelsiusSelected = true;
            CurrentTempMode = Celsius;

            _openWeather = WeatherWebServiceProvider.Instance;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChangedAsync;
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

        public ForecastInformationViewModel ForecastInformation
        {
            get => _forecastInformation;
            set
            {
                _forecastInformation = value;
                RaisePropertyChanged(() => ForecastInformation);
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

        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                _selectedCity = value;
                RaisePropertyChanged(() => SelectedCity);
            }
        }

        public bool IsFinishedLoading
        {
            get => _isFinishedLoading;
            set
            {
                _isFinishedLoading = value;
                RaisePropertyChanged(() => IsFinishedLoading);
            }
        }

        #endregion

        #region  Methods 

        public override async void ViewAppearing()
        {
            await Task.Run(async () =>
            {
                try
                {
                    NetworkAccess networkAccess = Connectivity.NetworkAccess;

                    if (networkAccess == NetworkAccess.None)
                    {
                        // Connection to internet is not available
                        //use cached information if it exists

                        if (Forecast == null || Forecast.Date == default)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                Forecast = new WeatherPageViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_NO_INTERNET };
                                TomorrowForecast = new WeatherPageViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_NO_INTERNET };
                                ForecastInformation = new ForecastInformationViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_NO_INTERNET };
                            });
                        }

                        IsFinishedLoading = true;
                    }

                    else
                    {
                        await LoadWeatherInformationAsync();
                    }
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
            });

            base.ViewAppearing();
        }

        private async Task LoadWeatherInformationAsync()
        {
            Location location = await Geolocation.GetLastKnownLocationAsync();
            IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
            Placemark placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                CountryName = placemark.CountryName;
                CountryCode = placemark.CountryCode;
                CityName = placemark.FeatureName;
            }

            using (UserDialogs.Instance.Loading("Loading..."))
            {
                GetWeatherReport(false, location);
            }

            UserDialogs.Instance.HideLoading();
        }

        public override void ViewAppeared()
        {
            MessagingCenter.Subscribe<WeatherViewModel>(this, "RefreshContent", (viewModel) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await LoadWeatherInformationAsync();
                });
            });
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            MessagingCenter.Unsubscribe<WeatherViewModel>(this, "RefreshContent");
            base.ViewDestroy(viewFinishing);
        }

        //the api refreshes every 2h so only after 1h have passed will the cache be updated 
        public IObservable<RootObject> GetForecastForFiveDay(Location location)
        {
            var currentCulture = CultureInfo.CurrentCulture.Parent.Name;

            return BlobCache.LocalMachine.GetAndFetchLatest("fiveDayForecast",
                async () => await _openWeather.GetForecastForFiveDaysAsync(location.Latitude, location.Longitude, currentCulture), (offset) =>
                    Connectivity.NetworkAccess != NetworkAccess.None && (DateTimeOffset.Now - offset).Hours > 1);
        }

        public IObservable<UvIndex> GetUltraVioletIndex(Location location)
        {
            return BlobCache.LocalMachine.GetAndFetchLatest("ultraVioletIndex",
            async () => await _openWeather.GetUltraVioletIndexAsync(location.Latitude, location.Longitude), (offset) =>
                Connectivity.NetworkAccess != NetworkAccess.None && (DateTimeOffset.Now - offset).Hours > 1);
        }

        private void GetWeatherReport(bool getCurrentDayReport, Location location)
        {
            try
            {
                if (!getCurrentDayReport)
                {
                    GetForecastForFiveDay(location).Subscribe(async (fiveDayForecast) =>
                                {
                                    await Task.Run(() =>
                                    {
                                        var currentForecast = new List<WeatherPageViewModel>();

                                        ForecastInformation = new ForecastInformationViewModel
                                        {
                                            ForecastInformationCollection = new MvxObservableCollection<IForecastInformation>()
                                        };

                                        if (!ValidateWeatherData(fiveDayForecast))
                                        {
                                            IsFinishedLoading = true;
                                            return;
                                        }

                                        foreach (List forecast in fiveDayForecast.list)
                                        {
                                            var date = Convert.ToDateTime(forecast.dt_txt, CultureInfo.InvariantCulture);
                                            WeatherPageViewModel selectedForecast = currentForecast.FirstOrDefault(x => x.Date.Date == date.Date);

                                            IWindInformation windInformation = Mvx.IoCProvider.Resolve<IWindInformation>();
                                            windInformation.SetWindInformation(date, forecast.wind);

                                            ITemperatureInformation temperature = Mvx.IoCProvider.Resolve<ITemperatureInformation>();
                                            temperature.SetTemperatureInformation(date, forecast.main);

                                            //waiting on an api update
                                            //ISysInformation sysInformation = Mvx.IoCProvider.Resolve<ISysInformation>();
                                            //sysInformation.SetSysInformation(date, forecast.sys);

                                            if (selectedForecast == null)
                                                currentForecast.Add(new WeatherPageViewModel(forecast, date, windInformation, temperature));
                                            else
                                            {
                                                selectedForecast.TemperatureInformationCollection.Add(temperature);
                                                selectedForecast.WindInformationCollection.Add(windInformation);
                                            }
                                        }

                                        SetViewModels(location, currentForecast);
                                        IsFinishedLoading = true;
                                    });
                                });
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Error getting weather report");
                Crashes.TrackError(ex);

                SetErrorGettingWeatherDetailsMessage();
                IsFinishedLoading = true;
            }
        }

        public bool ValidateWeatherData(RootObject fiveDayForecast)
        {
            if (fiveDayForecast == null || (fiveDayForecast.list == null && !fiveDayForecast.list.Any()))
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SetErrorGettingWeatherDetailsMessage();
                });

                return false;
            }

            return true;
        }

        private void SetErrorGettingWeatherDetailsMessage()
        {
            Forecast = new WeatherPageViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_GETTING_WEATHER_FORECAST };
            TomorrowForecast = new WeatherPageViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_GETTING_WEATHER_FORECAST };
            ForecastInformation = new ForecastInformationViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_GETTING_WEATHER_FORECAST };
        }

        private void SetViewModels(Location location, List<WeatherPageViewModel> currentForecast)
        {
            if (currentForecast.Any())
            {
                SetForecastInformation(currentForecast);
                SetCurrentForecast(location, currentForecast);
                SetTomorrowForecast(currentForecast);
            }

            else
            {
                SetErrorGettingWeatherDetailsMessage();
            }
        }

        private void SetTomorrowForecast(List<WeatherPageViewModel> currentForecast)
        {
            if (currentForecast.Count > 1)
            {
                TomorrowForecast = currentForecast[1];
                TomorrowForecast.CurrentHumidity = Math.Round(currentForecast[1].TemperatureInformationCollection.Average(x => x.WeatherMain.humidity), 2).ToString(CultureInfo.InvariantCulture);
                TomorrowForecast.CurentPressure = Math.Round(currentForecast[1].TemperatureInformationCollection.Average(x => x.WeatherMain.pressure), 2).ToString(CultureInfo.InvariantCulture);
            }

            else
                TomorrowForecast = new WeatherPageViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_GETTING_WEATHER_FORECAST };
        }

        private void SetCurrentForecast(Location location, List<WeatherPageViewModel> currentForecast)
        {
            Forecast = currentForecast[0];

            GetUltraVioletIndex(location).Subscribe((ultraVioletIndex) =>
            {
                Forecast.CurrentUvIndex = ultraVioletIndex?.value.ToString(CultureInfo.InvariantCulture);
            });

            Forecast.CurrentHumidity = currentForecast[0].TemperatureInformationCollection[0].WeatherMain.humidity.ToString();
            Forecast.CurentPressure = currentForecast[0].TemperatureInformationCollection[0].WeatherMain.pressure.ToString(CultureInfo.InvariantCulture);
        }

        private void SetForecastInformation(List<WeatherPageViewModel> currentForecast)
        {
            foreach (WeatherPageViewModel forecast in currentForecast)
            {
                ForecastInformation.ForecastInformationCollection.Add(new ForecastInformation(forecast.Date, forecast.TemperatureInformationCollection, forecast.WindInformationCollection[0], forecast.CurrentWeatherDescription));
            }
        }

        #endregion

        #region Events

        private async void Connectivity_ConnectivityChangedAsync(object sender, ConnectivityChangedEventArgs e)
        {
            NetworkAccess networkAccess = e.NetworkAccess;

            //reconnected to the internet, refresh!
            if (networkAccess == NetworkAccess.Internet)
            {
                await LoadWeatherInformationAsync();
            }
        }

        #endregion
    }
}
