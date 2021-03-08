namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeEditViewModel : BindableBase
    {
        public EmployeeEditViewModel(Employee employee)
        {
            Employee = employee;
            Employee.BeginEdit();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Employee.PostsEmployees.Clear();
            Employee.EmployeesInDepartments.Clear();
        }

        public Employee Employee { get; set; }

        #region Команды
        public ICommand EditEmployee => new DelegateCommand<Window>(empEditWindow =>
        {
            Employee.EndEdit();
            Edit();
            empEditWindow.Close();
        }, _ => Employee.IsValidationCollections() && Employee.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(empAddWindow =>
        {
            Employee.CancelEdit();
            empAddWindow.Close();
        });

        public ICommand AddPostInCollection => new DelegateCommand(() => Employee.PostsEmployees.Add(new Posts_employees()));

        public ICommand AddDepartmentInCollection => new DelegateCommand(() => Employee.EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeletePostFromCollection => new DelegateCommand<Posts_employees>(Employee.DeletePostFromCollection);

        public ICommand DeleteDepartmentFromCollection => new DelegateCommand<Employees_in_departments>(Employee.DeleteDepartmentFromCollection);
        #endregion

        private async void Edit() => await Employee.EditEmployee(Employee);
    }
}
