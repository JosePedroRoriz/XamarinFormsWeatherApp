using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WeatherWebservices.OpenWeatherModels;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class WindInformation : WeatherInformation
    {
        public WindInformation(DateTime date, Wind wind)
        {
            DayForecast = date;
            Wind = wind;
        }

        public Wind Wind { get; set; }
    }
}
