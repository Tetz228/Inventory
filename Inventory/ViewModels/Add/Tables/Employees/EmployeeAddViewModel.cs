namespace Inventory.ViewModels.Add.Tables.Employees
{
    using System;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeAddViewModel : BindableBase
    {
        public EmployeeAddViewModel()
        {
            PostsEmployees.Add(new Posts_employees());
            EmployeesInDepartments.Add(new Employees_in_departments());
        }

        #region Свойства
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ObservableCollection<Posts_employees> PostsEmployees { get; set; } = new();

        public ObservableCollection<Employees_in_departments> EmployeesInDepartments { get; set; } = new();
        #endregion

        #region Методы валидации данных
        private bool IsValidation()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                return false;

            if (string.IsNullOrWhiteSpace(FirstName))
                return false;

            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return false;

            if (!IsValidEmail(Email))
                return false;

            if (PostsEmployees.Count == 0)
                return false;

            if (EmployeesInDepartments.Count == 0)
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
        public ICommand AddEmployee => new DelegateCommand<Window>(empAddWindow =>
        {
            using var db = new InventoryEntities();

            var employee = new Employee
            {
                L_name = LastName,
                F_name = FirstName,
                M_name = MiddleName,
                Phone_number = PhoneNumber,
                Email = this.Email
            };

            db.Employees.Add(employee);
            db.SaveChanges();

            foreach (var post in PostsEmployees)
                post.Fk_employee = employee.Id_employee;

            db.Posts_employees.AddRange(PostsEmployees);
            db.SaveChanges();

            foreach (var department in EmployeesInDepartments)
                department.Fk_employee = employee.Id_employee;

            db.Employees_in_departments.AddRange(EmployeesInDepartments);
            db.SaveChanges();

            empAddWindow.Close();

        }, _ => IsValidation());

        public ICommand Cancel => new DelegateCommand<Window>(empAddWindow => empAddWindow.Close());

        public ICommand AddPostInCollection => new DelegateCommand(() => PostsEmployees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollection => new DelegateCommand<Posts_employees>((postEmp) => PostsEmployees.Remove(postEmp));

        public ICommand AddDepartmentInCollection => new DelegateCommand(() => EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollection => new DelegateCommand<Employees_in_departments>((empInDepart) => EmployeesInDepartments.Remove(empInDepart));
        #endregion
    }
}