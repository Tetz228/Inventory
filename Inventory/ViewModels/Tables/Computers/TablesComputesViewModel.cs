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
    using GraphicsCardsPage = Inventory.View.Pages.Tables.Computers.Accessories.GraphicsCardsPage;

    public class TablesComputesViewModel : BindableBase
    {
        public TablesComputesViewModel()
        {
            #region Инициализация объектов

            CurrentPage = SoketsPage = new SoketsPage();
            ManufacturersPage = new ManufacturersPage();
            HddsPage = new HddsPage();
            UnitsPage = new UnitsPage();
            TypesHddsPage = new TypesHddsPage();
            TypesSsdsPage = new TypesSsdsPage();
            TypesMemoryPage = new TypesMemoryPage();
            StatusesComputersPage = new StatusesComputersPage();
            OperatingSystemsPage = new OperatingSystemsPage();
            MotherboardsPage = new MotherboardsPage();
            ProcessorsPage = new ProcessorsPage();
            RamPage = new RamPage();
            SsdPage = new SsdPage();
            GraphicsCardsPage = new GraphicsCardsPage();
            PowerSuppliesPage = new PowerSuppliesPage();
            InventoryHddsPage = new InventoryHddsPage();
            InventoryGraphicsCardsPage = new InventoryGraphicsCardsPage();
            InventoryMotherboardsPage = new InventoryMotherboardsPage();
            InventorySsdsPage = new InventorySsdsPage();
            InventoryPowerSuppliesPag = new InventoryPowerSuppliesPage();
            InventoryProcessorsPage = new InventoryProcessorsPage();
            InventoryRamPage = new InventoryRamPage();

            #endregion
        }

        #region Свойства
        public Page CurrentPage { get; private set; }

        private ManufacturersPage ManufacturersPage { get; }

        private HddsPage HddsPage { get; }

        private TypesSsdsPage TypesSsdsPage { get; }

        private TypesMemoryPage TypesMemoryPage { get; }

        private UnitsPage UnitsPage { get; }

        private SoketsPage SoketsPage { get; }

        private TypesHddsPage TypesHddsPage { get; }

        private StatusesComputersPage StatusesComputersPage { get; }

        private OperatingSystemsPage OperatingSystemsPage { get; }

        private MotherboardsPage MotherboardsPage { get; }

        private ProcessorsPage ProcessorsPage { get; }

        private RamPage RamPage { get; }

        private SsdPage SsdPage { get; }

        private GraphicsCardsPage GraphicsCardsPage { get; }

        private InventoryHddsPage InventoryHddsPage { get; }

        private InventorySsdsPage InventorySsdsPage { get; }

        private InventoryMotherboardsPage InventoryMotherboardsPage { get; }

        private PowerSuppliesPage PowerSuppliesPage { get; }

        private InventoryGraphicsCardsPage InventoryGraphicsCardsPage { get; }

        private InventoryPowerSuppliesPage InventoryPowerSuppliesPag { get; }

        private InventoryProcessorsPage InventoryProcessorsPage { get; }

        private InventoryRamPage InventoryRamPage { get; }

        #endregion

        #region Команды
        public ICommand SelectSoketsPageCommand => new DelegateCommand(() => CurrentPage = SoketsPage);

        public ICommand SelectHddsPageCommand => new DelegateCommand(() => CurrentPage = HddsPage);

        public ICommand SelectUnitsPageCommand => new DelegateCommand(() => CurrentPage = UnitsPage);

        public ICommand SelectTypesHddPageCommand => new DelegateCommand(() => CurrentPage = TypesHddsPage);

        public ICommand SelectTablesManufacturersCommand => new DelegateCommand(() => CurrentPage = ManufacturersPage);

        public ICommand SelectTypesSsdPageCommand => new DelegateCommand(() => CurrentPage = TypesSsdsPage);

        public ICommand SelectOperatingSystemsPageCommand => new DelegateCommand(() => CurrentPage = OperatingSystemsPage);

        public ICommand SelectStatusesComputersPageCommand => new DelegateCommand(() => CurrentPage = StatusesComputersPage);

        public ICommand SelectTypesMemoryPageCommand => new DelegateCommand(() => CurrentPage = TypesMemoryPage);

        public ICommand SelectInventoryPowerSuppliesPageCommand => new DelegateCommand(() => CurrentPage = InventoryPowerSuppliesPag);

        public ICommand SelectInventoryProcessorsPageCommand => new DelegateCommand(() => CurrentPage = InventoryProcessorsPage);

        public ICommand SelectInventoryRamPageCommand => new DelegateCommand(() => CurrentPage = InventoryRamPage);

        public ICommand SelectInventoryHddsPageCommand => new DelegateCommand(() => CurrentPage = InventoryHddsPage);

        public ICommand SelectInventorySsdsPageCommand => new DelegateCommand(() => CurrentPage = InventorySsdsPage);

        public ICommand SelectInventoryMotherboardsPageCommand => new DelegateCommand(() => CurrentPage = InventoryMotherboardsPage);

        public ICommand SelectInventoryGraphicsCardsPageCommand => new DelegateCommand(() => CurrentPage = InventoryGraphicsCardsPage);

        public ICommand SelectMotherboardsPageCommand => new DelegateCommand(() => CurrentPage = MotherboardsPage);

        public ICommand SelectProcessorsPageCommand => new DelegateCommand(() => CurrentPage = ProcessorsPage);

        public ICommand SelectRamPageCommand => new DelegateCommand(() => CurrentPage = RamPage);

        public ICommand SelectSsdPageCommand => new DelegateCommand(() => CurrentPage = SsdPage);

        public ICommand SelectGraphicsCardsPageCommand => new DelegateCommand(() => CurrentPage = GraphicsCardsPage);

        public ICommand SelectPowerSuppliesPageCommand => new DelegateCommand(() => CurrentPage = PowerSuppliesPage);
        #endregion
    }
}
