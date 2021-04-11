namespace Inventory.Services
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class StringToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertStringToDouble(value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertStringToDouble(value);

        private static object ConvertStringToDouble(object value)
        {
            if (value == null)
                return null;

            string doubleString = value.ToString();

            return double.TryParse(doubleString, out double number) ? number : 0;
        }
    }
}
