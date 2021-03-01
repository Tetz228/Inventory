using System.Linq;

namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
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

        public string FilterSearch { get; set; } = "По ФИО";

        #region Свойства
        public ObservableCollection<Employee> Employees { get; private set; }

        private ICollectionView EmployeesCollection { get; }

        private string _searchEmployee;

        public string SearchEmployee
        {
            get => _searchEmployee;
            set
            {
                _searchEmployee = value;
                EmployeesCollection.Filter = obj =>
                {
                    if (obj is Employee employee)
                        switch (FilterSearch)
                        {
                            case "По должностям":
                                foreach (var postsEmployees in employee.Posts_employees)
                                    if (postsEmployees.Post.Name.ToLower().Contains(SearchEmployee.ToLower()))
                                        return postsEmployees.Post.Name.ToLower().Contains(SearchEmployee.ToLower());
                                break;
                            case "По отделам":
                                foreach (var employeesInDepartments in employee.Employees_in_departments)
                                    if (employeesInDepartments.Department.Name.ToLower().Contains(SearchEmployee.ToLower()))
                                        return employeesInDepartments.Department.Name.ToLower().Contains(SearchEmployee.ToLower());
                                break;
                            case "По ФИО":
                                return (employee.L_name + " " + employee.F_name + " " + employee.M_name).ToLower().Contains(SearchEmployee.ToLower());
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

        public ICommand ClickCommand => new DelegateCommand(() => FilterSearch = "По ФИО");

        public ICommand ClickCommand1 => new DelegateCommand(() => FilterSearch = "По должностям");

        public ICommand ClickCommand2 => new DelegateCommand(() => FilterSearch = "По отделам");

        public ICommand AddEmployee => new DelegateCommand(() =>
        {
            //using var db = new InventoryEntities();
            //var addDepartmentWindow = new DepartmentAddWindow();
            //var addDepartmentViewModel = new DepartmentAddViewModel();

            //addDepartmentWindow.DataContext = addDepartmentViewModel;
            //addDepartmentWindow.ShowDialog();

            //Employees = new ObservableCollection<Department>(db.Employees.ToList());
            //EmployeesCollection = CollectionViewSource.GetDefaultView(Employees);
        });

        public ICommand EditEmployee => new DelegateCommand<Employee>((employee) =>
        {
            //using var db = new InventoryEntities();
            //var editDepartmentWindow = new DepartmentEditWindow();
            //var editDepartmentViewModel = new DepartmentEditViewModel
            //{
            //    Department = depart
            //};

            //editDepartmentWindow.DataContext = editDepartmentViewModel;
            //editDepartmentWindow.ShowDialog();

            //Employees = new ObservableCollection<Department>(db.Departments.ToList());
            //EmployeesCollection = CollectionViewSource.GetDefaultView(Employees);
        }, (employee) => employee != null);

        public ICommand DeleteEmployee => new DelegateCommand<Employee>((employee) =>
        {
            //using var db = new InventoryEntities();
            //var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == employee.Id_department);

            //if (findDepartment == null)
            //{
            //    MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
            //    Employees = new ObservableCollection<Department>(db.Departments.ToList());
            //    return;
            //}

            //db.Departments.Remove(findDepartment);
            //db.SaveChanges();

            //Employees.Remove(employee);
        }, (employee) => employee != null);
        #endregion
    }
}
