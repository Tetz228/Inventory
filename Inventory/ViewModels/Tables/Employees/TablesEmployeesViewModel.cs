namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.View.Pages.Tables.Employees;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class TablesEmployeesViewModel : BindableBase
    {
        private const string USER_S = "Пользователь";

        public TablesEmployeesViewModel()
        {
            CurrentPage = EmployeesPage = new EmployeesPage();
            DepartmentsPage = new DepartmentsPage();
            PostsPage = new PostsPage();
            UsersPage = new UsersPage();
        }

        #region Свойства
        public Page CurrentPage { get; private set; }

        private DepartmentsPage DepartmentsPage { get; }

        private EmployeesPage EmployeesPage { get; }

        private PostsPage PostsPage { get; }

        private UsersPage UsersPage { get; }

        public bool OnAdmin { get; set; } = true/*User.AuthorizedUser.Role.Name != USER_S*/;
        #endregion

        #region Команды
        public ICommand SelectTablesDepartmentsCommand => new DelegateCommand(() => CurrentPage = DepartmentsPage);

        public ICommand SelectTablesEmployeesCommand => new DelegateCommand(() => CurrentPage = EmployeesPage);

        public ICommand SelectTablesPostsCommand => new DelegateCommand(() => CurrentPage = PostsPage);

        public ICommand SelectTablesUsersCommand => new DelegateCommand(() => CurrentPage = UsersPage);

        #endregion
    }
}
