using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public abstract class WeatherInformation
    {
        public DateTime DayForecast { get; set; }
    }
}
