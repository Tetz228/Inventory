namespace Inventory.ViewModels
{
    using System.Windows;

    using DevExpress.Mvvm;

    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.Pages.Tables.Computers;
    using Inventory.Pages.Tables.Employees;

    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            CurrentPage = Employees = new TablesEmployeesPage();
            Computers = new ComputersPage();
        }

        #region Свойства
        private TablesEmployeesPage Employees { get; }

        private ComputersPage Computers { get; }

        public Page CurrentPage { get; private set; }
        #endregion

        #region Команды
        public ICommand OpenPageComputers => new DelegateCommand(() => CurrentPage = Computers);

        public ICommand Exit => new DelegateCommand(() => Application.Current.Shutdown());

        public ICommand OpenPageEmployees => new DelegateCommand(() => CurrentPage = Employees);
        #endregion
    }
}
