using System;
using WeatherWebservices.OpenWeatherModels;
using XamarinFormsWeatherApp.Weather.Interfaces;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public interface ITemperatureInformation : IWeatherInformation
    {
        Main WeatherMain { get; set; }

        void SetTemperatureInformation(DateTime date, Main main);
    }
}
