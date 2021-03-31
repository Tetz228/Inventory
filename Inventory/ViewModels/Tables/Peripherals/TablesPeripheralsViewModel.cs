namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;

    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.View.Pages.Tables;
    using Inventory.View.Pages.Tables.Peripherals;

    public class TablesPeripheralsViewModel : BindableBase
    {
        public TablesPeripheralsViewModel()
        {
            CurrentPage = PeripheralsPage = new PeripheralsPage();
            TypesPeripheralsPage = new TypesPeripheralsPage();
            ManufacturersPage = new ManufacturersPage();
            InventoryNumbersPeripheralsPage = new InventoryNumbersPeripheralsPage();
            StatusesPeripheralsPage = new StatusesPeripheralsPage();
        }

        #region Свойства
        public Page CurrentPage { get; private set; }

        private StatusesPeripheralsPage StatusesPeripheralsPage { get; }

        private PeripheralsPage PeripheralsPage { get; }

        private InventoryNumbersPeripheralsPage InventoryNumbersPeripheralsPage { get; }

        private TypesPeripheralsPage TypesPeripheralsPage { get; }

        private ManufacturersPage ManufacturersPage { get; }
        #endregion

        #region Команды
        public ICommand SelectInventoryPeripheralsPageCommand => new DelegateCommand(() => CurrentPage = InventoryNumbersPeripheralsPage);

        public ICommand SelectTablesPeripheralsCommand => new DelegateCommand(() => CurrentPage = PeripheralsPage);

        public ICommand SelectStatusesPeripheralsCommand => new DelegateCommand(() => CurrentPage = StatusesPeripheralsPage);

        public ICommand SelectTablesTypesPeripheralsCommand => new DelegateCommand(() => CurrentPage = TypesPeripheralsPage);

        public ICommand SelectTablesManufacturersCommand => new DelegateCommand(() => CurrentPage = ManufacturersPage);
        #endregion
    }
}
