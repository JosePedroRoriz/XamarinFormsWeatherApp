using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WeatherWebservices.OpenWeatherModels;
using XamarinFormsWeatherApp.Weather.Interfaces;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class WindInformation : IWindInformation
    {
        public WindInformation(DateTime date, Wind wind)
        {
            Wind = wind;
            DayForecast = date;
        }

        public Wind Wind { get; set; }
        public DateTime DayForecast { get; set; }
    }
}
