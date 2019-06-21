using System;
using System.Collections.Generic;
using System.Text;
using WeatherWebservices.OpenWeatherModels;
using XamarinFormsWeatherApp.Weather.Interfaces;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class SysInformation : ISysInformation
    {
        private DateTime date;
        private Sys sys;

        public SysInformation(DateTime date, Sys sys)
        {
            this.date = date;
            this.sys = sys;
        }

        public Sys SystemInfo { get; set; }
        public DateTime DayForecast { get; set ; }
    }
}
