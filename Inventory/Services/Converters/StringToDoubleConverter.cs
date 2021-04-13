namespace Inventory.Services.Converters
{
    public class StringToDoubleConverter : BaseConverter
    {
        protected override object ConvertStringTo(object value)
        {
            if (value == null)
                return null;

            string doubleString = value.ToString();

            return double.TryParse(doubleString, out double number) ? number : 0;
        }
    }
}
