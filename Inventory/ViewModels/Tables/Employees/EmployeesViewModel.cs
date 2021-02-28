using System.Linq;

namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Windows.Input;

    public class EmployeesViewModel : BindableBase
    {
        public EmployeesViewModel()
        {
            using var db = new InventoryEntities();
            Employees = new ObservableCollection<Employee> (db.Employees.Include(employeePost => employeePost.Posts_employees
                                                                       .Select(post => post.Post))
                                                                       .Include(empDepart => empDepart.Employees_in_departments
                                                                       .Select(depart => depart.Department)));

            //EmployeesCollection = CollectionViewSource.GetDefaultView(Employees);
        }

        #region Свойства
        //public ObservableCollection<Employee> Employees { get; private set; }

        public ObservableCollection<Employee> Employees { get; set; }

        private string _searchEmployee;

        public string SearchEmployee
        {
            get => _searchEmployee;
            set
            {
                //_searchEmployee = value;
                //EmployeesCollection.Filter = obj =>
                //{
                //    if (obj is Employee department)
                //        return department.Name.ToLower().Contains(SearchEmployee.ToLower());

                //    return false;
                //};
                //EmployeesCollection.Refresh();
            }
        }

        private Employee _selectEmployee;
        
        public Employee SelectEmployee
        {
            get => _selectEmployee;
            set
            {
                _selectEmployee = value;
            }
        }

        #endregion

        #region Команды
        public ICommand DataGridMouseLeftButtonDown => new DelegateCommand(() => SelectEmployee = null);

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
