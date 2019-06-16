using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MvvmCross.ViewModels;
using WeatherWebservices.OpenWeatherModels;
using Xamarin.Forms;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class ForecastInformation : WeatherInformation
    {
        private DateTime date;
        private WindInformation windInformation;
        private MvxObservableCollection<WeatherWebservices.OpenWeatherModels.Weather> weatherCollection;

        public ForecastInformation(DateTime date, MvxObservableCollection<TemperatureInformation> temperatureInformationCollection, WindInformation windInformation, MvxObservableCollection<WeatherWebservices.OpenWeatherModels.Weather> weatherCollection)
        {
            DayForecast = date;

            MinTemp = temperatureInformationCollection[0].WeatherMain.temp_min;
            MaxTemp = temperatureInformationCollection[0].WeatherMain.temp_max;
            Humidity = Math.Round(temperatureInformationCollection.Average(x => x.WeatherMain.humidity), 2).ToString();

            TemperatureInformationCollection = temperatureInformationCollection;
            Description = weatherCollection[0].description;
            Pressure = Math.Round(temperatureInformationCollection.Average(x => x.WeatherMain.pressure), 2).ToString();
            WindSpeed = windInformation.Wind.speed;
        }



        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }


        public MvxObservableCollection<TemperatureInformation> TemperatureInformationCollection { get; set; }

        //part of the "hidden group"
        public double WindSpeed { get; set; }

        public string Humidity { get; set; }
        public string Pressure { get; set; }

        public bool IsVisible { get; set; }

        public string Description { get; set; }

        public FontAttributes FontWeight {get;set; }

    }
}
