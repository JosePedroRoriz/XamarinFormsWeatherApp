using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinFormsWeatherApp.Weather.Models;

namespace XamarinFormsWeatherApp.Weather.Interfaces
{
    public interface IForecastInformation
    {
        DateTime DayForecast { get; set; }
        string Description { get; set; }
        FontAttributes FontWeight { get; set; }
        string Humidity { get; set; }
        bool IsVisible { get; set; }
        double MaxTemp { get; set; }
        double MinTemp { get; set; }
        string Pressure { get; set; }
        IEnumerable<ITemperatureInformation> TemperatureInformationCollection { get; set; }
        IWindInformation WindInformation { get; set; }

        void SetForecastInformation(DateTime date, IEnumerable<ITemperatureInformation> temperatureInformation,
            IWindInformation windInformation, string weatherDesc);
    }
}
