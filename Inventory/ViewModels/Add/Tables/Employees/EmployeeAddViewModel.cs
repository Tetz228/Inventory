namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeAddViewModel : BindableBase
    {
        public EmployeeAddViewModel()
        {
            Employee = new Employee();
            Employee.PostsEmployees.Add(new Posts_employees());
            Employee.EmployeesInDepartments.Add(new Employees_in_departments());
        }

        public Employee Employee { get; set; }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Employee.PostsEmployees.Clear();
            Employee.EmployeesInDepartments.Clear();
        }

        #region Команды
        public ICommand AddEmployee => new DelegateCommand<Window>(empAddWindow =>
        {
            Add();
            empAddWindow.Close();
        }, _ => Employee.IsValidationCollections() && Employee.IsValidationProperties());

        private async void Add() => await Employee.AddEmployee(Employee);

        public ICommand Cancel => new DelegateCommand<Window>(empAddWindow => empAddWindow.Close());

        public ICommand AddPostInCollection => new DelegateCommand(() => Employee.PostsEmployees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollection => new DelegateCommand<Posts_employees>(postEmp => Employee.PostsEmployees.Remove(postEmp));

        public ICommand AddDepartmentInCollection => new DelegateCommand(() => Employee.EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollection => new DelegateCommand<Employees_in_departments>(empInDepart => Employee.EmployeesInDepartments.Remove(empInDepart));
        #endregion
    }
}