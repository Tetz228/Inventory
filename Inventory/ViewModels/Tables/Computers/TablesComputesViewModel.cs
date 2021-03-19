namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Pages.Tables;
    using Inventory.Pages.Tables.Computers;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TablesComputesViewModel : BindableBase
    {
        public TablesComputesViewModel()
        {
            CurrentPage = SoketsPage = new SoketsPage();
            ManufacturersPage = new ManufacturersPage();
        }

        #region Свойства
        public Page CurrentPage { get; private set; }

        private ManufacturersPage ManufacturersPage { get; }

        private SoketsPage SoketsPage { get; }
        #endregion

        #region Команды
        public ICommand SelectSoketsPage => new DelegateCommand(() => CurrentPage = SoketsPage);

        public ICommand SelectTablesManufacturers => new DelegateCommand(() => CurrentPage = ManufacturersPage);
        #endregion
    }
}
