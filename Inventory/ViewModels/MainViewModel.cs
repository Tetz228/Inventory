namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Pages;
    using System.Windows.Controls;
    using System.Windows.Input;

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

        public ICommand OpenPageEmployees => new DelegateCommand(() => CurrentPage = Employees);
        #endregion
    }
}
