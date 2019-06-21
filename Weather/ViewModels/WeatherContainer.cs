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
using XamarinFormsWeatherApp.Remove;
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

        private readonly IMvxNavigationService _navigationService;
        private IOpenWeather _openWeather;
        private bool _isFinishedLoading;

        private List<WeatherPageViewModel> currentForecast;

        #endregion

        #region Constructor

        public WeatherContainer(IMvxNavigationService navigation)
        {
            IsCelsiusSelected = true;
            CurrentTempMode = Celsius;

            _navigationService = navigation;
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

        #region Commands


        #endregion

        #region Private Methods 

        public override async void ViewAppearing()
        {
            await Task.Run(async () =>
            {
                try
                {
                    var networkAccess = Connectivity.NetworkAccess;

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
            var placemark = placemarks?.FirstOrDefault();

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
            {
                return Connectivity.NetworkAccess == NetworkAccess.None ? false : (DateTimeOffset.Now - offset).Hours > 1;
            });
        }

        public IObservable<UvIndex> GetUltraVioletIndex(Location location)
        {
            return BlobCache.LocalMachine.GetAndFetchLatest("ultraVioletIndex",
            async () => await _openWeather.GetUltraVioletIndexAsync(location.Latitude, location.Longitude), (offset) =>
            {
                return Connectivity.NetworkAccess == NetworkAccess.None ? false : (DateTimeOffset.Now - offset).Hours > 1;
            });
        }

        #endregion

        #region Events

        private void GetWeatherReport(bool getCurrentDayReport, Location location)
        {
            try
            {
                if (!getCurrentDayReport)
                {
                    var currentCulture = CultureInfo.CurrentCulture.Parent.Name;

                    GetForecastForFiveDay(location).Subscribe(async (fiveDayForecast) =>
                    {
                        await Task.Run(() =>
                            {
                                currentForecast = new List<WeatherPageViewModel>();

                                ForecastInformation = new ForecastInformationViewModel
                                {
                                    ForecastInformationCollection = new MvxObservableCollection<IForecastInformation>()
                                };

                                foreach (List forecast in fiveDayForecast.list)
                                {
                                    //date is in US format
                                    var date = Convert.ToDateTime(forecast.dt_txt, CultureInfo.InvariantCulture);
                                    WeatherPageViewModel selectedForecast = currentForecast.FirstOrDefault(x => x.Date.Date == date.Date);

                                    if (selectedForecast == null)
                                        currentForecast.Add(new WeatherPageViewModel(forecast, date));
                                    else
                                    {
                                        //selectedForecast.WeatherCollection.Add(WeatherFactory.GetWeatherInformation(forecast.weather[0]));
                                        //selectedForecast.TemperatureInformationCollection.Add(WeatherFactory.GetTemperatureInformation(date, forecast.main));
                                        ITemperatureInformation temperature = Mvx.IoCProvider.Resolve<ITemperatureInformation>();
                                        temperature.SetTemperatureInformation(date, forecast.main);
                                        selectedForecast.TemperatureInformationCollection.Add(temperature);
                                        selectedForecast.WindInformationCollection.Add(WeatherFactory.GetWindInformation(date, forecast.wind));
                                        selectedForecast.SystemInformationCollection.Add(WeatherFactory.GetSysInformation(date, forecast.sys));

                                        //selectedForecast.WeatherCollection.Add(WeatherFactory.GetWeatherInformation(forecast.weather[0]));
                                    }
                                }

                                foreach (WeatherPageViewModel forecast in currentForecast)
                                {
                                    ForecastInformation.ForecastInformationCollection.Add(new Models.ForecastInformation(forecast.Date, forecast.TemperatureInformationCollection, forecast.WindInformationCollection[0], forecast.CurrentWeatherDescription));
                                }

                                if (currentForecast.Any())
                                {
                                    Forecast = currentForecast[0];

                                    GetUltraVioletIndex(location).Subscribe((ultraVioletIndex) =>
                                    {
                                        Forecast.CurrentUvIndex = ultraVioletIndex?.value.ToString();
                                    });

                                    Forecast.CurrentHumidity = currentForecast[0].TemperatureInformationCollection[0].WeatherMain.humidity.ToString();
                                    Forecast.CurentPressure = currentForecast[0].TemperatureInformationCollection[0].WeatherMain.pressure.ToString();
                                    //Forecast.CurrentWeatherDescription = currentForecast[0].WeatherCollection[0].description;
                                }
                                //pass things as aprams and navigate, don t want this right now
                                //await _navigationService.Navigate<WeatherPageViewModel, WeatherPageViewModel>(Forecast);

                                if (currentForecast.Count > 2)
                                {
                                    TomorrowForecast = currentForecast[1];
                                    TomorrowForecast.CurrentHumidity = Math.Round(currentForecast[1].TemperatureInformationCollection.Average(x => x.WeatherMain.humidity), 2).ToString();
                                    TomorrowForecast.CurentPressure = Math.Round(currentForecast[1].TemperatureInformationCollection.Average(x => x.WeatherMain.pressure), 2).ToString();

                                    //using the 1st element for this one...
                                    //TomorrowForecast.CurrentWeatherDescription = currentForecast[1].WeatherCollection[0].description;
                                }

                                IsFinishedLoading = true;
                            });
                    });
                }
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Error getting weather report ");
                Crashes.TrackError(ex);

                //error getitng reports, show something to the user
                Forecast = new WeatherPageViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_GETTING_WEATHER_FORECAST };
                TomorrowForecast = new WeatherPageViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_GETTING_WEATHER_FORECAST };
                ForecastInformation = new ForecastInformationViewModel { IsDataValid = false, Message = Localizations.WeatherMainPageLocalization.ERROR_GETTING_WEATHER_FORECAST };

                IsFinishedLoading = true;
            }
        }

        private async void Connectivity_ConnectivityChangedAsync(object sender, ConnectivityChangedEventArgs e)
        {
            NetworkAccess networkAccess = e.NetworkAccess;

            //reconnected to the internet, refresh!
            if (networkAccess == NetworkAccess.Internet)
            {
                await LoadWeatherInformationAsync();
            }
        }

        public string GetCurrentUvIndex(UvIndex uvIndex)
        {
            return uvIndex?.value.ToString();
        }

        #endregion
    }
}
