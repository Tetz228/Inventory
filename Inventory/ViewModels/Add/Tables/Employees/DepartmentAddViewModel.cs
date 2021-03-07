namespace Inventory.ViewModels.Add.Tables.Employees
{
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

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
            Add();

            addWindow.Close();
        }, _ => Department.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion

        private async void Add() => await Department.AddDepartment(Department.Name);
    }
}
