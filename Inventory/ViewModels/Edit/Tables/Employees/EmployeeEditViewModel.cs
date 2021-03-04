namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeEditViewModel : BindableBase
    {
        public Employee Employee { get; set; }

        #region Методы валидации данных
        private bool IsValidation()
        {
            if (string.IsNullOrWhiteSpace(Employee.L_name))
                return false;

            if (string.IsNullOrWhiteSpace(Employee.F_name))
                return false;

            if (string.IsNullOrWhiteSpace(Employee.Phone_number))
                return false;

            if (!IsValidEmail(Employee.Email))
                return false;

            if (Employee.Posts_employees.Count == 0)
                return false;

            if (Employee.Employees_in_departments.Count == 0)
                return false;

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();

                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        #endregion

        #region Команды
        public ICommand EditEmployee => new DelegateCommand<Window>(empEditWindow =>
        {
            using var db = new InventoryEntities();
            var findEmployee = db.Employees.SingleOrDefault(emp => emp.Id_employee == Employee.Id_employee);

            if (findEmployee == null)
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении должности", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                findEmployee.Name = Post.Name;
                db.SaveChanges();
            }

            foreach (var post in PostsEmployees)
                post.Fk_employee = employee.Id_employee;

            db.Posts_employees.AddRange(PostsEmployees);
            db.SaveChanges();

            foreach (var department in EmployeesInDepartments)
                department.Fk_employee = employee.Id_employee;

            db.Employees_in_departments.AddRange(EmployeesInDepartments);
            db.SaveChanges();

            empEditWindow.Close();

        }, _ => IsValidation());

        public ICommand Cancel => new DelegateCommand<Window>(empEditWindow =>
        {
            PostsEmployees.Add(new Posts_employees());
            //empEditWindow.Close(); 

        });

        public ICommand AddPostInCollection => new DelegateCommand(() => PostsEmployees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollection => new DelegateCommand<Posts_employees>((postEmp) => PostsEmployees.Remove(postEmp));

        public ICommand AddDepartmentInCollection => new DelegateCommand(() => EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollection => new DelegateCommand<Employees_in_departments>((empInDepart) => EmployeesInDepartments.Remove(empInDepart));
        #endregion
    }
}
