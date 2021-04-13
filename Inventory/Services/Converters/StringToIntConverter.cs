namespace Inventory.Services.Converters
{
    public class StringToIntConverter : BaseConverter
    {
        protected override object ConvertStringTo(object value)
        {
            if (value == null)
                return null;

            string intString = value.ToString();

            return int.TryParse(intString, out int number) ? number : 0;
        }
    }
}
