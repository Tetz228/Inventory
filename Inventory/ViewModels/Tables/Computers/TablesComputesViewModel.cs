namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.View.Pages.Tables;
    using Inventory.View.Pages.Tables.Computers.Accessories;
    using Inventory.View.Pages.Tables.Computers.Computers;
    using Inventory.View.Pages.Tables.Computers.InventoryNumbers;
    using Inventory.View.Pages.Tables.Computers.Other;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TablesComputesViewModel : BindableBase
    {
        public TablesComputesViewModel() => CurrentPage = new ComputersPage();

        public Page CurrentPage { get; private set; }

        #region Команды
        public ICommand SelectSoketsPageCommand => new DelegateCommand(() => CurrentPage = new SoketsPage());

        public ICommand SelectHddsPageCommand => new DelegateCommand(() => CurrentPage = new HddsPage());

        public ICommand SelectUnitsPageCommand => new DelegateCommand(() => CurrentPage = new UnitsPage());

        public ICommand SelectTypesHddPageCommand => new DelegateCommand(() => CurrentPage = new TypesHddsPage());

        public ICommand SelectTablesManufacturersCommand => new DelegateCommand(() => CurrentPage = new ManufacturersPage());

        public ICommand SelectComputersCommand => new DelegateCommand(() => CurrentPage = new ComputersPage());

        public ICommand SelectTypesSsdPageCommand => new DelegateCommand(() => CurrentPage = new TypesSsdsPage());

        public ICommand SelectOperatingSystemsPageCommand => new DelegateCommand(() => CurrentPage = new OperatingSystemsPage());

        public ICommand SelectStatusesComputersPageCommand => new DelegateCommand(() => CurrentPage = new StatusesComputersPage());

        public ICommand SelectTypesMemoryPageCommand => new DelegateCommand(() => CurrentPage = new TypesMemoryPage());

        public ICommand SelectInventoryPowerSuppliesPageCommand => new DelegateCommand(() => CurrentPage = new InventoryPowerSuppliesPage());

        public ICommand SelectInventoryProcessorsPageCommand => new DelegateCommand(() => CurrentPage = new InventoryProcessorsPage());

        public ICommand SelectInventoryRamPageCommand => new DelegateCommand(() => CurrentPage = new InventoryRamPage());

        public ICommand SelectInventoryHddsPageCommand => new DelegateCommand(() => CurrentPage = new InventoryHddsPage());

        public ICommand SelectInventorySsdsPageCommand => new DelegateCommand(() => CurrentPage = new InventorySsdsPage());

        public ICommand SelectInventoryMotherboardsPageCommand => new DelegateCommand(() => CurrentPage = new InventoryMotherboardsPage());

        public ICommand SelectInventoryGraphicsCardsPageCommand => new DelegateCommand(() => CurrentPage = new InventoryGraphicsCardsPage());

        public ICommand SelectMotherboardsPageCommand => new DelegateCommand(() => CurrentPage = new MotherboardsPage());

        public ICommand SelectProcessorsPageCommand => new DelegateCommand(() => CurrentPage = new ProcessorsPage());

        public ICommand SelectRamPageCommand => new DelegateCommand(() => CurrentPage = new RamPage());

        public ICommand SelectSsdPageCommand => new DelegateCommand(() => CurrentPage = new SsdPage());

        public ICommand SelectGraphicsCardsPageCommand => new DelegateCommand(() => CurrentPage = new GraphicsCardsPage());

        public ICommand SelectPowerSuppliesPageCommand => new DelegateCommand(() => CurrentPage = new PowerSuppliesPage());
        #endregion
    }
}
