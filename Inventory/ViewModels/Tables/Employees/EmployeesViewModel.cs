﻿namespace Inventory.ViewModels.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Employees;
    using Inventory.View.Edit.Tables.Employees;
    using Inventory.ViewModels.Edit.Tables.Employees;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class EmployeesViewModel : BaseViewModel<Employee>
    {
        public EmployeesViewModel() : base(Employees, RefreshCollection) => RefreshCollection();

        public static ObservableCollection<Employee> Employees { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "ФИО":
                        {
                            Employees.Sort(employee => employee.L_name, SortDirection);
                            break;
                        }
                    case "Почта":
                        {
                            Employees.Sort(employee => employee.Email, SortDirection);
                            break;
                        }
                    case "Номер телефона":
                        {
                            Employees.Sort(employee => employee.Phone_number, SortDirection);
                            break;
                        }
                        //case "Должности":
                        //    {
                        //        Employees.Sort(employee => employee.Posts_employees, SortDirection);
                        //        break;
                        //    }
                        //case "Отделы":
                        //    {
                        //        Employees.Sort(employee => employee.Employees_in_departments, SortDirection);
                        //        break;
                        //    }
                }
            }
        }
        
        public ICommand AddEmployeeCommand => new DelegateCommand(() =>
        {
            var addEmployeeWindow = new EmployeeAddWindow();
            addEmployeeWindow.ShowDialog();
        });

        public ICommand EditEmployeeCommand => new DelegateCommand<Employee>(employee =>
        {
            var editEmployeeViewModel = new EmployeeEditViewModel(employee);
            var editEmployeeWindow = new EmployeeEditWindow();
            editEmployeeWindow.DataContext = editEmployeeViewModel;
            editEmployeeWindow.Closing += editEmployeeViewModel.OnWindowClosing;
            editEmployeeWindow.ShowDialog();
        }, employee => employee != null);

        public ICommand DeleteEmployeeCommand => new DelegateCommand<Employee>(selectEmployee =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить сотрудника:\nФИО - {selectEmployee.L_name} {selectEmployee.F_name} {selectEmployee.M_name};\nномер телефона - {selectEmployee.Phone_number};\nпочта - {selectEmployee.Email}?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if (Services.Delete<Employee>(selectEmployee.Id_employee))
                Employees.Remove(selectEmployee);
        }, selectHdd => selectHdd != null);

        public static void RefreshCollection()
        {
            Employees.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Employees.AsNoTracking().Include(employeePost => employeePost.Posts_employees
                    .Select(post => post.Post))
                .Include(empDepart => empDepart.Employees_in_departments
                    .Select(depart => depart.Department)))
            {
                Employees.Add(item);
            }
            Employees.Sort(employee => employee.L_name);
        }
    }
}