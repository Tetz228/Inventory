namespace Inventory.ViewModels.Add.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class DepartmentAddViewModel : BindableBase
    {
        public DepartmentAddViewModel()
        {
            Department = new Department();
        }

        public Department Department { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Department.AddDepartment(Department.Name);
            addWindow.Close();
        }, _ => Department.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
