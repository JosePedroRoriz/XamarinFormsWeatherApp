using System;
using WeatherWebservices.OpenWeatherModels;

namespace XamarinFormsWeatherApp.Weather.Interfaces
{
    public interface IWindInformation : IWeatherInformation
    {
        Wind Wind { get; set; }

        void SetWindInformation(DateTime date, Wind wind);
    }
}
