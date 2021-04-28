namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeAddViewModel : BindableBase
    {
        public EmployeeAddViewModel()
        {
            using var db = new InventoryEntities();

            Posts = new ObservableCollection<Post>(db.Posts);
            Departments = new ObservableCollection<Department>(db.Departments);

            Employee.Posts_employees.Add(new Posts_employees());
            Employee.Employees_in_departments.Add(new Employees_in_departments());
        }

        public Employee Employee { get; } = new();

        public ObservableCollection<Post> Posts { get; }

        public ObservableCollection<Department> Departments { get; }

        #region Команды
        public ICommand AddEmployeeCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            Services.Add(Employee);
            EmployeesViewModel.RefreshCollection();
            empAddWindow.Close();
        }, _ => Services.IsValidationProperties(Employee.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(empAddWindow => empAddWindow.Close());

        public ICommand AddPostInCollectionCommand => new DelegateCommand(() => Employee.Posts_employees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollectionCommand => new DelegateCommand<Posts_employees>(postEmp => Employee.Posts_employees.Remove(postEmp));

        public ICommand AddDepartmentInCollectionCommand => new DelegateCommand(() => Employee.Employees_in_departments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollectionCommand => new DelegateCommand<Employees_in_departments>(empInDepart => Employee.Employees_in_departments.Remove(empInDepart));
        #endregion
    }
}