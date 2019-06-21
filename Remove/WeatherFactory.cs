using System;
using XamarinFormsWeatherApp.Weather.Interfaces;
using XamarinFormsWeatherApp.Weather.Models;

namespace XamarinFormsWeatherApp.Remove
{
    public static class WeatherFactory
    {
        //public static ITemperatureInformation GetTemperatureInformation(DateTime date, WeatherWebservices.OpenWeatherModels.Main main)
        //{
        //    return new TemperatureInformation(date, main);
        //}

        public static IWindInformation GetWindInformation(DateTime date, WeatherWebservices.OpenWeatherModels.Wind wind)
        {
            return new WindInformation(date, wind);
        }

        public static ISysInformation GetSysInformation(DateTime date, WeatherWebservices.OpenWeatherModels.Sys sys)
        {
            return new SysInformation(date, sys);
        }

        //public static IWeather GetWeatherInformation(WeatherWebservices.OpenWeatherModels.Weather weather)
        //{
        //    return new WeatherInformation();
        //}
    }
}
