namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;

    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.View.Pages.Tables.Computers;
    using Inventory.View.Pages.Tables.Computers.Computers;
    using Inventory.View.Pages.Tables.Employees;
    using Inventory.View.Pages.Tables.Peripherals;

    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            Peripherals = new TablesPeripheralsPage();
            CurrentPage = Computers = new TablesComputersPage();
            Employees = new TablesEmployeesPage();
        }

        #region Свойства
        private TablesEmployeesPage Employees { get; }

        private TablesPeripheralsPage Peripherals { get; }

        private TablesComputersPage Computers { get; }

        public Page CurrentPage { get; private set; }
        #endregion

        #region Команды
        public ICommand OpenPageComputers => new DelegateCommand(() => CurrentPage = Computers);

        public ICommand OpenPagePeripherals => new DelegateCommand(() => CurrentPage = Peripherals);

        public ICommand Exit => new DelegateCommand(() => Application.Current.Shutdown());

        public ICommand OpenPageEmployees => new DelegateCommand(() => CurrentPage = Employees);
        #endregion
    }
}
