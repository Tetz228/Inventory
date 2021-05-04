namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;

    public class DepartmentEditViewModel : BindableBase, IEditableObject
    {
        public DepartmentEditViewModel(Department department)
        {
            Department = department;
            BeginEdit();
        }

        public Department Department { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Department.Id_department, Department);
            DepartmentsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Department.ErrorCollection));

        #region Откат изменений
        private Department _selectDepartment;

        public void BeginEdit()
        {
            _selectDepartment = new Department
            {
                Id_department = Department.Id_department,
                Name = Department.Name,
            };
        }

        public void EndEdit() => _selectDepartment = null;

        public void CancelEdit()
        {
            if (_selectDepartment == null)
                return;

            Department.Id_department = _selectDepartment.Id_department;
            Department.Name = _selectDepartment.Name;
        }
        #endregion
    }
}
