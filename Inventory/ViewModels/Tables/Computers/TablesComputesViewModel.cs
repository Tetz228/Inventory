namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;

    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.View.Pages.Tables;
    using Inventory.View.Pages.Tables.Computers;

    public class TablesComputesViewModel : BindableBase
    {
        public TablesComputesViewModel()
        {
            CurrentPage = SoketsPage = new SoketsPage();
            ManufacturersPage = new ManufacturersPage();
            HddsPage = new HddsPage();
            UnitsPage = new UnitsPage();
            TypesHddsPage = new TypesHddsPage();
            TypesSsdsPage = new TypesSsdsPage();
            TypesMemoryPage = new TypesMemoryPage();
            StatusesComputersPage = new StatusesComputersPage();
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

        #endregion

        #region Команды
        public ICommand SelectSoketsPageCommand => new DelegateCommand(() => CurrentPage = SoketsPage);

        public ICommand SelectHddsPageCommand => new DelegateCommand(() => CurrentPage = HddsPage);

        public ICommand SelectUnitsPageCommand => new DelegateCommand(() => CurrentPage = UnitsPage);

        public ICommand SelectTypesHddPageCommand => new DelegateCommand(() => CurrentPage = TypesHddsPage);

        public ICommand SelectTablesManufacturersCommand => new DelegateCommand(() => CurrentPage = ManufacturersPage);

        public ICommand SelectTypesSsdPageCommand => new DelegateCommand(() => CurrentPage = TypesSsdsPage);

        public ICommand SelectStatusesComputersPageCommand => new DelegateCommand(() => CurrentPage = StatusesComputersPage);

        public ICommand SelectTypesMemoryPageCommand => new DelegateCommand(() => CurrentPage = TypesMemoryPage);
        #endregion
    }
}
