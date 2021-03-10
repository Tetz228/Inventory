namespace Inventory.ViewModels.Tables.Employees
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Pages.Tables.Employees;

    internal class TablesEmployeesViewModel : BindableBase
    {
        public TablesEmployeesViewModel()
        {
            DepartmentsPage = new DepartmentsPage();
            EmployeesPage = new EmployeesPage();
            PostsPage = new PostsPage();
            AccountsPage = new AccountsPage();
            RolesPage = new RolesPage();
        }

        #region Свойства
        public Page CurrentPage { get; private set; }

        private DepartmentsPage DepartmentsPage { get; }

        private EmployeesPage EmployeesPage { get; }

        private PostsPage PostsPage { get; }

        private AccountsPage AccountsPage { get; }

        private RolesPage RolesPage { get; }
        #endregion

        #region Команды
        public ICommand SelectTablesDepartments => new DelegateCommand(() => CurrentPage = DepartmentsPage);

        public ICommand SelectTablesEmployees => new DelegateCommand(() => CurrentPage = EmployeesPage);

        public ICommand SelectTablesPosts => new DelegateCommand(() => CurrentPage = PostsPage);

        public ICommand SelectTablesAccounts => new DelegateCommand(() => CurrentPage = AccountsPage);

        public ICommand SelectTablesRoles => new DelegateCommand(() => CurrentPage = RolesPage);
        #endregion
    }
}
