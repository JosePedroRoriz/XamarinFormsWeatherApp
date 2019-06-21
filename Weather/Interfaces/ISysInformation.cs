using System;
using System.Collections.Generic;
using System.Text;
using WeatherWebservices.OpenWeatherModels;

namespace XamarinFormsWeatherApp.Weather.Interfaces
{
    public interface ISysInformation : IWeatherInformation
    {
        Sys SystemInfo { get; set; }
    }
}
