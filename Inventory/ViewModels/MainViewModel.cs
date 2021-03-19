namespace Inventory.ViewModels
{
    using System.Windows;

    using DevExpress.Mvvm;

    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.Pages.Tables.Computers;
    using Inventory.Pages.Tables.Employees;
    using Inventory.Pages.Tables.Peripherals;

    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            CurrentPage = Peripherals = new TablesPeripheralsPage();
            TablesComputers = new TablesComputersPage();
            Employees = new TablesEmployeesPage();
        }

        #region Свойства
        private TablesEmployeesPage Employees { get; }

        private TablesPeripheralsPage Peripherals { get; }

        private TablesComputersPage TablesComputers { get; }

        public Page CurrentPage { get; private set; }
        #endregion

        #region Команды
        public ICommand OpenPageComputers => new DelegateCommand(() => CurrentPage = TablesComputers);

        public ICommand OpenPagePeripherals => new DelegateCommand(() => CurrentPage = Peripherals);

        public ICommand Exit => new DelegateCommand(() => Application.Current.Shutdown());

        public ICommand OpenPageEmployees => new DelegateCommand(() => CurrentPage = Employees);
        #endregion
    }
}
