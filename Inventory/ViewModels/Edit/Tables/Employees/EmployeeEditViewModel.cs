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
            EditPostEmployee(Employee.Id_employee);
            EditEmployeeInDepartment(Employee.Id_employee);

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

        public void EditEmployeeInDepartment(int idEmployee)
        {
            using var db = new InventoryEntities();

            foreach (var department in Employee.Employees_in_departments)
            {
                if (department.Id_employee_in_department == 0)
                {
                    department.Fk_employee = idEmployee;
                    db.Employees_in_departments.Add(department);
                    db.SaveChanges();
                }
                else
                {
                    var employeesInDepartments = db.Employees_in_departments.Where(empDepart => empDepart.Id_employee_in_department == department.Id_employee_in_department).ToList();
                    foreach (var item in employeesInDepartments)
                    {
                        item.Fk_department = department.Fk_department;
                        db.SaveChanges();
                    }
                }
            }
        }

        public void EditPostEmployee(int idEmployee)
        {
            using var db = new InventoryEntities();

            foreach (var post in Employee.Posts_employees)
            {
                if (post.Id_post_employee == 0)
                {
                    post.Fk_employee = idEmployee;
                    db.Posts_employees.Add(post);
                    db.SaveChanges();
                }
                else
                {
                    var postEmployee = db.Posts_employees.Where(postEmp => postEmp.Id_post_employee == post.Id_post_employee).ToList();
                    foreach (var item in postEmployee)
                    {
                        item.Fk_post = post.Fk_post;
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
