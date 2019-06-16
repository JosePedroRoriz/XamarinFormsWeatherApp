using System;
using System.Collections.Generic;
using System.Text;
using WeatherWebservices.OpenWeatherModels;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class SysInformation : WeatherInformation
    {
        public SysInformation(DateTime date, Sys sys)
        {
            DayForecast = date;
            SystemInfo = sys;
        }

        public Sys SystemInfo { get; set; }
    }
}
