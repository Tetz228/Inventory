namespace Inventory.Services
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertStringToInt(value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertStringToInt(value);

        private static object ConvertStringToInt(object value)
        {
            if (value == null)
                return null;

            string intString = value.ToString();

            return int.TryParse(intString, out int number) ? number : 0;
        }
    }
}
