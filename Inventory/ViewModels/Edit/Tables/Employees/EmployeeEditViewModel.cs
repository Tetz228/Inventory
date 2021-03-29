namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;

    public class EmployeeEditViewModel : BindableBase, IEditableObject
    {
        public EmployeeEditViewModel(Employee employee)
        {
            using var db = new InventoryEntities();
            Posts_employees.CollectionPosts = new List<Post>(db.Posts);
            Employees_in_departments.CollectionDepartments = new List<Department>(db.Departments);

            Employee = employee;
            BeginEdit();
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            Employee.PostsEmployees.Clear();
            Employee.EmployeesInDepartments.Clear();
            CancelEdit();
        }

        public Employee Employee { get; }

        #region Команды
        public ICommand EditEmployeeCommand => new DelegateCommand<Window>(empEditWindow =>
        {
            EndEdit();
            Services.Edit(Employee.Id_employee, Employee);
            Posts_employees.EditPostEmployee(Employee.Id_employee);
            Employees_in_departments.EditEmployeeInDepartment(Employee.Id_employee);
            EmployeesViewModel.RefreshCollection();
            empEditWindow.Close();
        }, _ => Employee.IsValidationCollections() && Services.IsValidationProperties(Employee.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            CancelEdit();
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

        #region Откат изменений
        private Employee _selectEmployee;

        public void BeginEdit()
        {
            _selectEmployee = new Employee
            {
                Id_employee = Employee.Id_employee,
                L_name = Employee.L_name,
                F_name = Employee.F_name,
                M_name = Employee.M_name,
                Phone_number = Employee.Phone_number,
                Email = Employee.Email
            };
        }

        public void EndEdit()
        {
            _selectEmployee = null;
        }

        public void CancelEdit()
        {
            if (_selectEmployee == null)
                return;

            Employee.Id_employee = _selectEmployee.Id_employee;
            Employee.L_name = _selectEmployee.L_name;
            Employee.F_name = _selectEmployee.F_name;
            Employee.M_name = _selectEmployee.M_name;
            Employee.Phone_number = _selectEmployee.Phone_number;
            Employee.Email = _selectEmployee.Email;
        }
        #endregion
    }
}
