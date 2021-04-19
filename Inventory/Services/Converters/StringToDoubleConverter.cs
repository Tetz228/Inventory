namespace Inventory.Services.Converters
{
    using System.Globalization;
    using System.Linq;

    public class StringToDoubleConverter : BaseConverter
    {
        protected override object ConvertStringTo(object value)
        {
            if (value == null)
                return null;

            if (double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.CurrentCulture, out double number))
                return value.ToString();

            return 0;
        }

        protected override object ConvertBackStringTo(object value)
        {
            if (value == null)
                return null;

            double.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.CurrentCulture, out double number);

            return number;
        }
    }
}
