namespace Inventory.ViewModels.Tables.Employees
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.View.Pages.Tables.Employees;

    internal class TablesEmployeesViewModel : BindableBase
    {
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
        #endregion

        #region Команды
        public ICommand SelectTablesDepartments => new DelegateCommand(() => CurrentPage = DepartmentsPage);

        public ICommand SelectTablesEmployees => new DelegateCommand(() => CurrentPage = EmployeesPage);

        public ICommand SelectTablesPosts => new DelegateCommand(() => CurrentPage = PostsPage);

        public ICommand SelectTablesUsers => new DelegateCommand(() => CurrentPage = UsersPage);

        #endregion
    }
}
