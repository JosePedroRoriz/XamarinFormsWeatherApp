using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsWeatherApp.MainContainer.ViewModels
{
    public class MainContainerViewModel : MvxViewModel
    {
        #region Commands

        public IMvxAsyncCommand SearchCityWeatherCommand { get; set; }



        #endregion
    }
}
