﻿namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Pages.Tables;
    using Inventory.Pages.Tables.Peripherals;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TablesPeripheralsViewModel : BindableBase
    {
        public TablesPeripheralsViewModel()
        {
            PeripheralsPage = new PeripheralsPage();
            TypesPeripheralsPage = new TypesPeripheralsPage();
            ManufacturersPage = new ManufacturersPage();
        }


        #region Свойства
        public Page CurrentPage { get; private set; }

        private PeripheralsPage PeripheralsPage { get; }

        private TypesPeripheralsPage TypesPeripheralsPage { get; }

        private ManufacturersPage ManufacturersPage { get; }
        #endregion

        #region Команды
        public ICommand SelectTablesPeripherals => new DelegateCommand(() => CurrentPage = PeripheralsPage);

        public ICommand SelectTablesTypesPeripherals => new DelegateCommand(() => CurrentPage = TypesPeripheralsPage);

        public ICommand SelectTablesManufacturers => new DelegateCommand(() => CurrentPage = ManufacturersPage);
        #endregion
    }
}
