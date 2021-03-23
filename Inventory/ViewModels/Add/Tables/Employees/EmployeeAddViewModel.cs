namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeAddViewModel : BindableBase
    {
        public EmployeeAddViewModel()
        {
            Employee = new Employee();

            using var db = new InventoryEntities();
            Posts_employees.CollectionPosts = new List<Post>(db.Posts);
            Employees_in_departments.CollectionDepartments = new List<Department>(db.Departments);

            Employee.PostsEmployees.Add(new Posts_employees());
            Employee.EmployeesInDepartments.Add(new Employees_in_departments());
        }

        public Employee Employee { get; }

        public void OnWindowClosing(object sender, CancelEventArgs args)
        {
            Employee.PostsEmployees.Clear();
            Employee.EmployeesInDepartments.Clear();
        }

        #region Команды
        public ICommand AddEmployeeCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            Employee.AddEmployee(Employee);
            empAddWindow.Close();
        }, _ => Employee.IsValidationCollections() && Employee.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(empAddWindow => empAddWindow.Close());

        public ICommand AddPostInCollectionCommand => new DelegateCommand(() => Employee.PostsEmployees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollectionCommand => new DelegateCommand<Posts_employees>(postEmp => Employee.PostsEmployees.Remove(postEmp));

        public ICommand AddDepartmentInCollectionCommand => new DelegateCommand(() => Employee.EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollectionCommand => new DelegateCommand<Employees_in_departments>(empInDepart => Employee.EmployeesInDepartments.Remove(empInDepart));
        #endregion
    }
}