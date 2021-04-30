namespace Inventory.Services.Converters
{
    using System;
    using System.Windows;

    public class StringToDataTimeConverter : BaseConverter
    {
        protected override object ConvertStringTo(object value) => ((DateTime)value).ToString("dd.MM.yyyy");

        protected override object ConvertBackStringTo(object value) => DependencyProperty.UnsetValue;
    }
}
