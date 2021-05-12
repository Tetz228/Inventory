namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.View.Pages.Tables.Employees;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Inventory.Model;

    public class TablesEmployeesViewModel : BindableBase
    {
        private const string USER_S = "Пользователь";

        public TablesEmployeesViewModel() => CurrentPage = new EmployeesPage();

        public Page CurrentPage { get; private set; }

        public bool OnAdmin { get; set; } = User.AuthorizedUser.Role.Name != USER_S;

        #region Команды
        public ICommand SelectTablesDepartmentsCommand => new DelegateCommand(() => CurrentPage = new DepartmentsPage());

        public ICommand SelectTablesEmployeesCommand => new DelegateCommand(() => CurrentPage = new EmployeesPage());

        public ICommand SelectTablesPostsCommand => new DelegateCommand(() => CurrentPage = new PostsPage());

        public ICommand SelectTablesUsersCommand => new DelegateCommand(() => CurrentPage = new UsersPage());

        #endregion
    }
}
