namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeEditViewModel : BindableBase
    {
        public EmployeeEditViewModel(Employee employee)
        {
            using var db = new InventoryEntities();
            Posts = new ObservableCollection<Post>(db.Posts);
            Departments = new ObservableCollection<Department>(db.Departments);

            Employee = employee;
            Employee.BeginEdit();
        }

        public Employee Employee { get; }

        public ObservableCollection<Post> Posts { get; set; }

        public ObservableCollection<Department> Departments { get; set; }

        #region Команды
        public ICommand EditEmployeeCommand => new DelegateCommand<Window>(empEditWindow =>
        {
            Employee.EndEdit();
            Services.Edit(Employee.Id_employee, Employee);
            EmployeesViewModel.RefreshCollection();
            empEditWindow.Close();
        }, _ => Services.IsValidationProperties(Employee.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            Employee.CancelEdit();
            empAddWindow.Close();
        });

        public ICommand AddPostInCollectionCommand => new DelegateCommand(() => Employee.Posts_employees.Add(new Posts_employees()));

        public ICommand AddDepartmentInCollectionCommand => new DelegateCommand(() => Employee.Employees_in_departments.Add(new Employees_in_departments()));

        public ICommand DeletePostFromCollectionCommand => new DelegateCommand<Posts_employees>(postEmp => Employee.Posts_employees.Remove(postEmp));

        public ICommand DeleteDepartmentFromCollectionCommand => new DelegateCommand<Employees_in_departments>(empInDepart => Employee.Employees_in_departments.Remove(empInDepart));
        #endregion
    }
}
