using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinFormsWeatherApp.Weather.ViewModels;

namespace XamarinFormsWeatherApp.ViewModels
{
    public class MvxMainViewModel : MvxViewModel
    {
        #region Variables

        private readonly IMvxNavigationService _navigationService;

        #endregion

        public MvxMainViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region Events

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            MvxNotifyTask.Create(async () => await this.InitializeViewModels());
        }

        private async Task InitializeViewModels()
        {
           //add vms to initialize
        }

        #endregion
    }
}
