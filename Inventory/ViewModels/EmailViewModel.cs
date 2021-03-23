using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels
{
    using System.Windows.Input;

    using DevExpress.Mvvm;

    public class EmailViewModel:BindableBase
    {
        public string Email { get; set; }

        public ICommand FurtherCommand => new DelegateCommand(() =>
        {
            
        });
    }
}
