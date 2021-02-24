using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels
{
    using System.Windows.Input;

    using DevExpress.Mvvm;

    public class DepartmentAddViewModel:BindableBase
    {
        public Action CloseWindow { get; set; }

        public DepartmentAddViewModel(Action closeWindow)
        {
            CloseWindow = closeWindow;
        }

        public ICommand Cancel => new DelegateCommand(() => CloseWindow());
    }
}
