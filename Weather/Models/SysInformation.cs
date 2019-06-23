using System;
using WeatherWebservices.OpenWeatherModels;
using XamarinFormsWeatherApp.Weather.Interfaces;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class SysInformation : ISysInformation
    {
        public SysInformation(DateTime date, Sys sys)
        {
            DayForecast = date;
            SystemInfo = sys;
        }

        public Sys SystemInfo { get; set; }
        public DateTime DayForecast { get; set ; }
    }
}
