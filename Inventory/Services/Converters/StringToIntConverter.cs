namespace Inventory.Services.Converters
{
    using System.Globalization;

    public class StringToIntConverter : BaseConverter
    {
        protected override object ConvertStringTo(object value)
        {
            if (value == null)
                return null;

            return int.Parse(value.ToString());
        }

        protected override object ConvertBackStringTo(object value)
        {
            if (value == null)
                return null;

            string intString = value.ToString();

            return int.TryParse(intString, out int number) ? number : 0;
        }
    }
}
