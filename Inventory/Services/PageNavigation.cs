namespace Inventory.Services
{
    using System;
    using System.Windows.Controls;

    public class PageNavigation
    {
        public event Action<Page> OnPageChanged;

        public void Navigate(Page obj) => OnPageChanged?.Invoke(obj);
    }
}
