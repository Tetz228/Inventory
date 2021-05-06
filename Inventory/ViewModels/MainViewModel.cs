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
    using Inventory.View.Pages.Tables.DispensingComputers;
    using Inventory.View.Pages.Tables.DispensingPeripherals;

    public class MainViewModel : BindableBase
    {
        public MainViewModel() => CurrentPage = new TablesComputersPage();

        #region Свойства

        public Page CurrentPage { get; private set; }

        public string TitleWindow { get; set; } = "Главное окно";
        #endregion

        #region Команды
        public ICommand OpenPageComputers => new DelegateCommand(() =>
        {
            CurrentPage = new TablesComputersPage();
            TitleWindow = "Главное окно -> Компьютеры";
        });

        public ICommand OpenPageEmployees => new DelegateCommand(() =>
        {
            CurrentPage = new TablesEmployeesPage();
            TitleWindow = "Главное окно -> Сотрудники";
        });

        public ICommand OpenDispensingPeripherals => new DelegateCommand(() =>
        {
            CurrentPage = new DispensingPeripheralsPage();
            TitleWindow = "Главное окно -> Выдачи периферии";
        });

        public ICommand OpenDispensingComputers => new DelegateCommand(() =>
        {
            CurrentPage = new DispensingComputersPage();
            TitleWindow = "Главное окно -> Выдачи компьютеров";
        });

        public ICommand OpenPagePeripherals => new DelegateCommand(() =>
        {
            CurrentPage = new TablesPeripheralsPage();
            TitleWindow = "Главное окно -> Периферии";
        });

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

        
        #endregion
    }
}
