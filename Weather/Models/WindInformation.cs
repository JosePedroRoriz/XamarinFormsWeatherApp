using System;
using WeatherWebservices.OpenWeatherModels;
using XamarinFormsWeatherApp.Weather.Interfaces;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class WindInformation : IWindInformation
    {
        public Wind Wind { get; set; }
        public DateTime DayForecast { get; set; }

        public void SetWindInformation(DateTime date, Wind wind)
        {
            Wind = wind;
            DayForecast = date;
        }
    }
}
