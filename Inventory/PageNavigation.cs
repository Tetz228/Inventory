using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory
{
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
