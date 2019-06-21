using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using XamarinFormsWeatherApp.Weather.Interfaces;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public abstract class WeatherInformation : IWeatherInformation
    {
        public DateTime DayForecast { get; set; }
    }
}
