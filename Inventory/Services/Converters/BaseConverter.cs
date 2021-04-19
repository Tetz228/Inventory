namespace Inventory.Services.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public abstract class BaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => ConvertStringTo(value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => ConvertBackStringTo(value);

        protected abstract object ConvertStringTo(object value);

        protected abstract object ConvertBackStringTo(object value);
    }
}
