using System;
using WeatherWebservices.OpenWeatherModels;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class TemperatureInformation : ITemperatureInformation
    {
        public Main WeatherMain { get; set; }
        public DateTime DayForecast { get ; set; }

        public void SetTemperatureInformation(DateTime date, Main main)
        {
            DayForecast = date;
            WeatherMain = main;
        }
    }
}
