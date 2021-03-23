﻿namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using System.Collections.Generic;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeEditViewModel : BindableBase
    {
        public EmployeeEditViewModel(Employee employee)
        {
            using var db = new InventoryEntities();
            Posts_employees.CollectionPosts = new List<Post>(db.Posts);
            Employees_in_departments.CollectionDepartments = new List<Department>(db.Departments);

            Employee = employee;
            Employee.BeginEdit();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Employee.PostsEmployees.Clear();
            Employee.EmployeesInDepartments.Clear();
            Employee.CancelEdit();
        }

        public Employee Employee { get; }

        #region Команды
        public ICommand EditEmployeeCommand => new DelegateCommand<Window>(empEditWindow =>
        {
            Employee.EndEdit();
            Employee.EditEmployee(Employee);
            empEditWindow.Close();
        }, _ => Employee.IsValidationCollections() && Employee.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            Employee.CancelEdit();
            empAddWindow.Close();
        });

        public ICommand AddPostInCollectionCommand => new DelegateCommand(() => Employee.PostsEmployees.Add(new Posts_employees()));

        public ICommand AddDepartmentInCollectionCommand => new DelegateCommand(() => Employee.EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeletePostFromCollectionCommand => new DelegateCommand<Posts_employees>(selectPostEmp =>
        {
            if (selectPostEmp.Id_post_employee != 0)
                if (MessageBoxResult.Yes != MessageBox.Show("Вы действительно хотите удалить должность сотрудника? Удаленную должность будет невозможно восстановить.",
                    "Удаление заданной должности", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
            Posts_employees.DeletePostEmployee(selectPostEmp);
        });

        public ICommand DeleteDepartmentFromCollectionCommand => new DelegateCommand<Employees_in_departments>(selectEmpInDepart =>
        {
            if (selectEmpInDepart.Id_employee_in_department != 0)
                if (MessageBoxResult.Yes != MessageBox.Show("Вы действительно хотите удалить отдел сотрудника? Удаленный отдел будет невозможно восстановить.",
                "Удаление заданного отдела", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
            Employees_in_departments.DeleteEmployeeDepartment(selectEmpInDepart);
        });
        #endregion
    }
}
