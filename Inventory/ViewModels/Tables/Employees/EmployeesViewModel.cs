namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class EmployeesViewModel : BindableBase
    {
        public EmployeesViewModel()
        {
            using var db = new InventoryEntities();
            Employees = new ObservableCollection<Employee>(db.Employees.Include(employeePost => employeePost.Posts_employees
                                                                       .Select(post => post.Post))
                                                                       .Include(empDepart => empDepart.Employees_in_departments
                                                                       .Select(depart => depart.Department))
                                                                       .Include(account => account.Accounts
                                                                       .Select(role => role.User.Roles_users)));

            EmployeesCollection = CollectionViewSource.GetDefaultView(Employees);

            EmployeesCollection.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Employee.Posts_employees)));
            EmployeesCollection.SortDescriptions.Add(new SortDescription(nameof(Employee.L_name), ListSortDirection.Ascending));
        }

        private string _employeesFilter = string.Empty;

        public string EmployeesFilter
        {
            get
            {
                return _employeesFilter;
            }
            set
            {
                _employeesFilter = value;
                EmployeesCollection.Filter = obj =>
                {
                    if (obj is Employee employee)
                    {
                        return employee.L_name.Contains(EmployeesFilter) || employee.F_name.Contains(EmployeesFilter) ||
                               employee.Email.Contains(EmployeesFilter) || employee.Phone_number.Contains(EmployeesFilter) ||
                               ContainsCollectionPostsEmployees(employee) || ContainsCollectionEmployeesInDepartments(employee);
                    }

                    return false;
                };
                EmployeesCollection.Refresh();
            }
        }

        #region Свойства
        public static ObservableCollection<Employee> Employees { get; set; }

        private ICollectionView EmployeesCollection { get; set; }

        private bool ContainsCollectionPostsEmployees(Employee employee) => employee.Posts_employees.Select(postsEmployees => postsEmployees.Post.Name.Contains(EmployeesFilter)).FirstOrDefault();

        private bool ContainsCollectionEmployeesInDepartments(Employee employee) => employee.Employees_in_departments.Select(employeesInDepartments => employeesInDepartments.Department.Name.Contains(EmployeesFilter)).FirstOrDefault();

        private string _search;

        
        public Employee SelectEmployee { get; set; }
        #endregion

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectEmployee = null);

        #region Действия
        public ICommand AddEmployee => new DelegateCommand(() =>
        {
            var addEmployeeWindow = new EmployeeAddWindow();

            addEmployeeWindow.ShowDialog();
        });

        /// <summary>Событие при клике на заголовок в View</summary>
        public void Initialize(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            if (columnHeader.Content.ToString() == "ФИО")
                Employees = new ObservableCollection<Employee>(Employees.OrderBy(lname => lname.L_name));
        }

        public ICommand EditEmployee => new DelegateCommand<Employee>(employee =>
        {
            using var db = new InventoryEntities();

            var editEmployeeViewModel = new EmployeeEditViewModel(employee);
            Employee.EmployeesInDepartments = new ObservableCollection<Employees_in_departments>(employee.Employees_in_departments);
            Employee.PostsEmployees = new ObservableCollection<Posts_employees>(employee.Posts_employees);

            var editEmployeeWindow = new EmployeeEditWindow
            {
                DataContext = editEmployeeViewModel
            };

            editEmployeeWindow.Closing += editEmployeeViewModel.OnWindowClosing;
            editEmployeeWindow.ShowDialog();

        }, employee => employee != null);

        public ICommand DeleteEmployee => new DelegateCommand<Employee>(Employee.DeleteEmployee, selectEmployee => selectEmployee != null);

        public ICommand RefreshList => new DelegateCommand(Employee.Refresh);
        #endregion
        #endregion
    }
}
