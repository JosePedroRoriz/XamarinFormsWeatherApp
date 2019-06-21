using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsWeatherApp.Converters
{
    public class HideGridElementOnNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                GridUnitType auto = GridUnitType.Auto;
                if (parameter != null)
                {
                    Enum.TryParse<GridUnitType>((string)parameter, true, out auto);
                }

                if (value != null)
                {
                    if (value is string gridValue)
                    {
                        bool d = string.IsNullOrEmpty(gridValue);
                        return d == true ? new GridLength(0, GridUnitType.Absolute) : new GridLength(1, auto);
                    }
                }
                return  new GridLength(0, GridUnitType.Absolute);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
