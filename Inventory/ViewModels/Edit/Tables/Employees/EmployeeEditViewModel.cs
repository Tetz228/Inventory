namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeEditViewModel : BindableBase
    {
        public EmployeeEditViewModel(Employee employee)
        {
            using var db = new InventoryEntities();
            Posts = new ObservableCollection<Post>(db.Posts);
            Departments = new ObservableCollection<Department>(db.Departments);

            Employee = employee;
            Employee.BeginEdit();
        }

        public Employee Employee { get; }

        public ObservableCollection<Post> Posts { get; set; }

        public ObservableCollection<Department> Departments { get; set; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Employee.CancelEdit();

        #region Команды
        public ICommand EditEmployeeCommand => new DelegateCommand<Window>(empEditWindow =>
        {
            Employee.EndEdit();
            Services.Edit(Employee.Id_employee, Employee);
            EditPostsEmployee(Employee.Id_employee);
            EditEmployeeInDepartments(Employee.Id_employee);

            EmployeesViewModel.RefreshCollection();
            empEditWindow.Close();
        }, _ => Services.IsValidationProperties(Employee.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(empAddWindow =>
        {
            Employee.CancelEdit();
            empAddWindow.Close();
        });

        public ICommand AddPostInCollectionCommand => new DelegateCommand(() => Employee.Posts_employees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollectionCommand => new DelegateCommand<Posts_employees>(selectPostEmp =>
        {
            if (selectPostEmp.Id_post_employee != 0)
            {
                if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить должность \"{selectPostEmp.Post.Name}\" у сотрудника? Это действие нельзя отменить.",
                    "Удаление заданного отдела", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
                Services.Delete<Posts_employees>(selectPostEmp.Id_post_employee);
            }
            Employee.Posts_employees.Remove(selectPostEmp);
        });

        public ICommand AddDepartmentInCollectionCommand => new DelegateCommand(() => Employee.Employees_in_departments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollectionCommand => new DelegateCommand<Employees_in_departments>(selectEmpInDepart =>
        {
            if (selectEmpInDepart.Id_employee_in_department != 0)
            {
                if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить отдел \"{selectEmpInDepart.Department.Name}\" у сотрудника? Это действие нельзя отменить.",
                    "Удаление заданного отдела", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    return;
                Services.Delete<Employees_in_departments>(selectEmpInDepart.Id_employee_in_department);
            }
            Employee.Employees_in_departments.Remove(selectEmpInDepart);
        });

        #endregion

        private void EditEmployeeInDepartments(int idEmployee)
        {
            using var db = new InventoryEntities();

            foreach (var item in Employee.Employees_in_departments)
            {
                if (item.Id_employee_in_department == 0)
                {
                    item.Fk_employee = idEmployee;
                    db.Employees_in_departments.Add(item);
                }
                else
                {
                    var employeeInDepartment = db.Employees_in_departments.FirstOrDefault(employeesInDepartments => employeesInDepartments.Id_employee_in_department == item.Id_employee_in_department);
                    if (employeeInDepartment != null)
                        employeeInDepartment.Fk_department = item.Fk_department;
                }
            }
            db.SaveChanges();
        }

        private void EditPostsEmployee(int idEmployee)
        {
            using var db = new InventoryEntities();

            foreach (var item in Employee.Posts_employees)
            {
                if (item.Id_post_employee == 0)
                {
                    item.Fk_employee = idEmployee;
                    db.Posts_employees.Add(item);
                }
                else
                {
                    var postInEmployee = db.Posts_employees.FirstOrDefault(postsEmployees => postsEmployees.Id_post_employee == item.Id_post_employee);
                    if (postInEmployee != null)
                        postInEmployee.Fk_post = item.Fk_post;
                }
            }
            db.SaveChanges();
        }
    }
}
