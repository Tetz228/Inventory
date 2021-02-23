namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.Pages;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class MainViewModel : BindableBase
    {
        public TablesEmployeesPage Emloyees { get; }
        public ComputersPage Computers { get; }

        public Page CurrentPage { get; set; }

        public MainViewModel()
        {
            Emloyees = new TablesEmployeesPage();
            Computers = new ComputersPage();

            CurrentPage = Emloyees;
        }

        public ICommand OpenPageComputers => new DelegateCommand(() => CurrentPage = Computers);

        public ICommand OpenPageEmloyees => new DelegateCommand(() => CurrentPage = Emloyees);
    }
}
