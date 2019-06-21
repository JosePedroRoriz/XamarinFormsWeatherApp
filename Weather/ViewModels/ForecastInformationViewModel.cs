using MvvmCross.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinFormsWeatherApp.Weather.Interfaces;
using XamarinFormsWeatherApp.Weather.Models;

namespace XamarinFormsWeatherApp.Weather.ViewModels
{
    public class ForecastInformationViewModel : WeatherViewModel
    {
        private bool _isVisible;
        private MvxObservableCollection<IForecastInformation> _forecastInformationCollection;
        private ForecastInformation _oldforecastInformation;
        private ForecastInformation _selectedItem;

        public ForecastInformationViewModel()
        {
            IsDataValid = true;
            ToggleWeatherDataCommand = new Command(ToggleWeatherDataCommandExecute);
        }

        #region Properties 

        public MvxObservableCollection<IForecastInformation> ForecastInformationCollection
        {
            get => _forecastInformationCollection;
            set
            {
                _forecastInformationCollection = value;
                RaisePropertyChanged(() => ForecastInformationCollection);
            }
        }

        public ForecastInformation SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        #endregion

        #region Command

        public Command ToggleWeatherDataCommand { get; set; }

        private void ToggleWeatherDataCommandExecute()
        {
            if (_oldforecastInformation == SelectedItem)
            {
                SelectedItem.IsVisible = !SelectedItem.IsVisible;
                SelectedItem.FontWeight = SelectedItem.FontWeight == FontAttributes.Bold ? FontAttributes.None : FontAttributes.Bold;
                UpdateForecastItem(SelectedItem);
            }
            else
            {
                if (_oldforecastInformation != null)
                {
                    _oldforecastInformation.IsVisible = false;
                    _oldforecastInformation.FontWeight = FontAttributes.None;
                    SelectedItem.FontWeight = FontAttributes.Bold;
                    UpdateForecastItem(_oldforecastInformation);

                }

                SelectedItem.IsVisible = true;
                SelectedItem.FontWeight = FontAttributes.Bold;
                UpdateForecastItem(SelectedItem);
            }
            _oldforecastInformation = SelectedItem;
        }

        #endregion

        private void UpdateForecastItem(ForecastInformation forecastInformation)
        {
            var Index = ForecastInformationCollection.IndexOf(forecastInformation);
            ForecastInformationCollection.Remove(forecastInformation);
            ForecastInformationCollection.Insert(Index, forecastInformation);
        }
    }
}
