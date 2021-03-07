﻿namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
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

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Department.EndEdit();
            Edit();
            editWindow.Close();
        }, _ => Department.Validation());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Department.CancelEdit();
            editWindow.Close();
        });
        #endregion

        private async void Edit() => await Department.EditDepartment(Department);
    }
}
