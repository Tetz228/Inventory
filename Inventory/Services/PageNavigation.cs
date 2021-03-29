namespace Inventory.Services
{
    using System;
    using System.Windows.Controls;

    public class PageNavigation
    {
        public event Action<Page> OnPangeChanged;

        public void Navigate(Page obj)
        {
            OnPangeChanged?.Invoke(obj);
        }
    }
}
