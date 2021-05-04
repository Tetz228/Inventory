namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;

    public class DepartmentAddViewModel : BindableBase
    {
        public Department Department { get; } = new();
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Department);
            DepartmentsViewModel.Departments.Add(Department);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Department.ErrorCollection));
    }
}
