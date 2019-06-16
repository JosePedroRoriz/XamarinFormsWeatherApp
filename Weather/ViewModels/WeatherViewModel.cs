using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinFormsWeatherApp.Weather.ViewModels
{
    public abstract class WeatherViewModel : MvxViewModel
    {
        private bool _isDataValid;
        private string _message;
        private bool _isBusy;

        public WeatherViewModel()
        {
            RefreshData = new Command(RefreshDataExecute);
        }

        #region Properties 

        public bool IsDataValid
        {
            get => _isDataValid;
            set
            {
                _isDataValid = value;
                RaisePropertyChanged(() => IsDataValid);
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        #endregion

        #region Commands

        public Command RefreshData { get; set; }

        public void RefreshDataExecute()
        {
            MainThread.BeginInvokeOnMainThread(() => { MessagingCenter.Send<WeatherViewModel>(this, "RefreshContent"); });
            MainThread.BeginInvokeOnMainThread(() => { MessagingCenter.Send<WeatherViewModel>(this, "RefreshFinished"); });
        }

        #endregion
    }
}
