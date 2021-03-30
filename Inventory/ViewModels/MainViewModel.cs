namespace Inventory.ViewModels
{
    using DevExpress.Mvvm;
    using Inventory.View.Pages.Tables.Computers;
    using Inventory.View.Pages.Tables.Employees;
    using Inventory.View.Pages.Tables.Peripherals;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View;

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

        public ICommand ChangePasswordCommand => new DelegateCommand(() =>
        {
            if (User.AuthorizedUser != null)
                UsersInteraction.OnWindowChangePassword(User.AuthorizedUser.Id_user);
        });

        public ICommand ChangeUserCommand => new DelegateCommand<Window>(mainWindow =>
        {
            var authWindow = new AuthorizationWindow();
            mainWindow.Close();
            authWindow.Show();
        });

        public ICommand ExitCommand => new DelegateCommand(() => Application.Current.Shutdown());

        public ICommand OpenPageEmployees => new DelegateCommand(() => CurrentPage = Employees);
        #endregion
    }
}
