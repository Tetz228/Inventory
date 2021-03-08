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
    using System.Threading.Tasks;
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
        }

        private string FilterSearch { get; set; } = "По ФИО";

        #region Свойства
        public static ObservableCollection<Employee> Employees { get; private set; }

        private ICollectionView EmployeesCollection { get; set; }

        private string _search;

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                EmployeesCollection.Filter = obj =>
                {
                    if (obj is Employee employee)
                        switch (FilterSearch)
                        {
                            case "По должностям":
                                foreach (var postsEmployees in employee.Posts_employees.Where(postsEmployees => postsEmployees.Post.Name.ToLower().Contains(Search.ToLower())))
                                    return postsEmployees.Post.Name.ToLower().Contains(Search.ToLower());
                                break;
                            case "По отделам":
                                foreach (var employeesInDepartments in employee.Employees_in_departments.Where(employeesInDepartments => employeesInDepartments.Department.Name.ToLower().Contains(Search.ToLower())))
                                    return employeesInDepartments.Department.Name.ToLower().Contains(Search.ToLower());
                                break;
                            case "По ФИО":
                                return (employee.L_name + " " + employee.F_name + " " + employee.M_name).ToLower().Contains(Search.ToLower());
                            case "По почте":
                                return employee.Email.ToLower().Contains(Search.ToLower());
                            case "По номеру телефона":
                                return employee.Phone_number.ToLower().Contains(Search.ToLower());
                        }
                    return false;
                };
                EmployeesCollection.Refresh();
            }
        }

        public Employee SelectEmployee { get; set; }
        #endregion

        #region Команды
        public ICommand ListViewMouseLeftButtonDown => new DelegateCommand(() => SelectEmployee = null);

        #region Фильтры поиска
        public ICommand SearchByFlm => new DelegateCommand(() => FilterSearch = "По ФИО");

        public ICommand SearchByEmails => new DelegateCommand(() => FilterSearch = "По почте");

        public ICommand SearchByPhoneNumbers => new DelegateCommand(() => FilterSearch = "По номеру телефона");

        public ICommand SearchByPosts => new DelegateCommand(() => FilterSearch = "По должностям");

        public ICommand SearchByDepartments => new DelegateCommand(() => FilterSearch = "По отделам");
        #endregion

        #region Действия
        public ICommand AddEmployee => new DelegateCommand(() =>
        {
            var addEmployeeWindow = new EmployeeAddWindow();

            addEmployeeWindow.ShowDialog();
        });

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
