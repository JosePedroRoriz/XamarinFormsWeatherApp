using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WeatherWebservices.OpenWeatherModels;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class TemperatureInformation : WeatherInformation
    {
        public TemperatureInformation(DateTime date, Main main)
        {
            DayForecast = date;
            WeatherMain = main;
        }

        public Main WeatherMain { get; set; }
    }
}
