using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Tables.Computers
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Pages.Tables;

    public class TablesComputesViewModel:BindableBase
    {
        public TablesComputesViewModel()
        {
            CurrentPage = ManufacturersPage = new ManufacturersPage();
        }


        #region Свойства
        public Page CurrentPage { get; private set; } 

        private ManufacturersPage ManufacturersPage { get; }
        #endregion

        #region Команды


        public ICommand SelectTablesManufacturers => new DelegateCommand(() => CurrentPage = ManufacturersPage);
        #endregion
    }
}
