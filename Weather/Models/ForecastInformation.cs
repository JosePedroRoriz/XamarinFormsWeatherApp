using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using XamarinFormsWeatherApp.Weather.Interfaces;

namespace XamarinFormsWeatherApp.Weather.Models
{
    public class ForecastInformation : IWeatherInformation, IForecastInformation
    {
        public void SetForecastInformation(DateTime date, IEnumerable<ITemperatureInformation> temperatureInformation, IWindInformation windInformation, string weatherDesc)
        {
            DayForecast = date;

            IEnumerable<ITemperatureInformation> temperatureInformationCollection = temperatureInformation.ToList();
            TemperatureInformationCollection = temperatureInformationCollection;
            WindInformation = windInformation;
            Description = weatherDesc;
            SetTemperatureValues(temperatureInformationCollection);
        }

        private void SetTemperatureValues(IEnumerable<ITemperatureInformation> temperatureInformation)
        {
            MinTemp = temperatureInformation.FirstOrDefault().WeatherMain.temp_min;
            MaxTemp = temperatureInformation.FirstOrDefault().WeatherMain.temp_max;

            Pressure = Math.Round(temperatureInformation.Average(x => x.WeatherMain.pressure), 2).ToString(CultureInfo.InvariantCulture);
            Humidity = Math.Round(temperatureInformation.Average(x => x.WeatherMain.humidity), 2).ToString(CultureInfo.InvariantCulture);
        }

        public IWindInformation WindInformation { get; set; }

        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }

        public IEnumerable<ITemperatureInformation> TemperatureInformationCollection { get; set; }

        //part of the "hidden group"

        public string Humidity { get; set; }
        public string Pressure { get; set; }
        public bool IsVisible { get; set; }
        public string Description { get; set; }
        public FontAttributes FontWeight { get; set; }
        public DateTime DayForecast { get; set; }
    }
}
