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
            TypesHddPage = new TypesHddPage();
        }

        #region Свойства
        public Page CurrentPage { get; private set; }

        private ManufacturersPage ManufacturersPage { get; }

        private HddsPage HddsPage { get; }

        private UnitsPage UnitsPage { get; }

        private SoketsPage SoketsPage { get; }

        private TypesHddPage TypesHddPage { get; }
        
        #endregion

        #region Команды
        public ICommand SelectSoketsPage => new DelegateCommand(() => CurrentPage = SoketsPage);

        public ICommand SelectHddsPage => new DelegateCommand(() => CurrentPage = HddsPage);

        public ICommand SelectUnitsPage => new DelegateCommand(() => CurrentPage = UnitsPage);

        public ICommand SelectTypesHddPage => new DelegateCommand(() => CurrentPage = TypesHddPage);

        public ICommand SelectTablesManufacturers => new DelegateCommand(() => CurrentPage = ManufacturersPage);
        #endregion
    }
}
