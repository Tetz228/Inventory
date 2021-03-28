namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class DepartmentEditViewModel : BindableBase
    {
        public DepartmentEditViewModel(Department department)
        {
            Department = department;
            Department.BeginEdit();
        }

        public Department Department { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Department.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Department.EndEdit();
            Services.Edit(Department.Id_department, Department);
            Department.RefreshCollection();
            editWindow.Close();
        }, _ => Department.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            Department.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
