using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using WeatherWebservices.OpenWeatherModels;
using Xamarin.Forms;

namespace XamarinFormsWeatherApp.Weather.ViewModels
{
    public class WeatherPageViewModel : MvxViewModel
    {
        #region Variables

        private DateTime _date;
        private Wind _wind;
        private MvxObservableCollection<Wind> _windCollection;

        private WeatherWebservices.OpenWeatherModels.Weather _weather;
        private string _country;
        private string _location;
        private Main _weatherMain;
        private ImageSource _imageSource;
        private MvxObservableCollection<WeatherWebservices.OpenWeatherModels.Weather> _weatherCollection;
        private MvxObservableCollection<Main> _weatherMainCollection;
        private MvxObservableCollection<Clouds> _cloudsCollection;
        private MvxObservableCollection<DateTime> _dateCollection;

        #endregion

        #region Constructor
        public WeatherPageViewModel(RootObject rootObject)
        {
            CurrentWind = rootObject.wind;
            Country = rootObject.sys.country;
            WeatherMain = rootObject.main;
        }

        /// <summary>
        /// List is an OpenWeatherReport class
        /// </summary>
        /// <param name="reportList"></param>
        public WeatherPageViewModel(List reportList, DateTime date)
        {
            Date = date;
            CurrentWind = reportList.wind;

            WindCollection = new MvxObservableCollection<Wind> { reportList.wind };
            WeatherCollection = new MvxObservableCollection<WeatherWebservices.OpenWeatherModels.Weather> { reportList.weather[0] };
            WeatherMainCollection = new MvxObservableCollection<Main> { reportList.main };
            CloudsCollection = new MvxObservableCollection<Clouds> { reportList.clouds };
            DateCollection = new MvxObservableCollection<DateTime> { date };
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

        public MvxObservableCollection<DateTime> DateCollection
        {
            get => _dateCollection;
            set
            {
                _dateCollection = value;
                RaisePropertyChanged(() => DateCollection);
            }
        }

        public Wind CurrentWind
        {
            get => _wind;
            set
            {
                _wind = value;
                RaisePropertyChanged(() => CurrentWind);
            }
        }

        public MvxObservableCollection<Wind> WindCollection
        {
            get => _windCollection;
            set
            {
                _windCollection = value;
                RaisePropertyChanged(() => WindCollection);
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

        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                RaisePropertyChanged(() => Country);
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                RaisePropertyChanged(() => Location);
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

        public MvxObservableCollection<Main> WeatherMainCollection
        {
            get => _weatherMainCollection;
            set
            {
                _weatherMainCollection = value;
                RaisePropertyChanged(() => WeatherMainCollection);
            }
        }

        public ImageSource WeatherIcon
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                RaisePropertyChanged(() => WeatherIcon);
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
        #endregion
    }
}
