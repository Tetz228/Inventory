namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.View.Pages.Tables;
    using Inventory.View.Pages.Tables.Peripherals;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TablesPeripheralsViewModel : BindableBase
    {
        public TablesPeripheralsViewModel() => CurrentPage = new PeripheralsPage();

        public Page CurrentPage { get; private set; }

        #region Команды
        public ICommand SelectInventoryPeripheralsPageCommand => new DelegateCommand(() => CurrentPage = new InventoryPeripheralsPage());

        public ICommand SelectTablesPeripheralsCommand => new DelegateCommand(() => CurrentPage = new PeripheralsPage());

        public ICommand SelectTablesTypesPeripheralsCommand => new DelegateCommand(() => CurrentPage = new TypesPeripheralsPage());

        public ICommand SelectTablesManufacturersCommand => new DelegateCommand(() => CurrentPage = new ManufacturersPage());
        #endregion
    }
}
