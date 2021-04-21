namespace Inventory.Services.Converters
{
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

            return int.TryParse(value.ToString(), out int number) ? number : 0;
        }
    }
}
