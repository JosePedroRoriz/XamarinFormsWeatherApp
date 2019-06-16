using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormsWeatherApp.Weather.ViewModels;

namespace XamarinFormsWeatherApp.Weather.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    //without this it the layout assumes its a tabbedPosition.tab and that brings a big padding to the top
    [MvxTabbedPagePresentation(TabbedPosition.Root, NoHistory = false)]
    public partial class WeatherMainPage : MvxTabbedPage<WeatherContainer>
    {
        public WeatherMainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}