using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace XamarinFormsWeatherApp.Converters
{
    public class UpperCaseFirstLetter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            string transformedString = "";
         
            foreach (var item in ((string)value).Split(' '))
            {
                transformedString += char.ToUpperInvariant(item[0]) + item.Substring(1) + " ";
            }

            return transformedString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
