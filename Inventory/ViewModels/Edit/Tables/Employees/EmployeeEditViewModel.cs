namespace Inventory.ViewModels.Edit.Tables.Employees
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;

    public class EmployeeEditViewModel : BindableBase
    {
        #region Свойства
        public Employee Employee { get; set; }

        public ObservableCollection<Posts_employees> PostsEmployees { get; set; }

        public ObservableCollection<Employees_in_departments> EmployeesInDepartments { get; set; }

        public List<Post> CollectionPosts { get; set; }

        public List<Department> CollectionDepartments { get; set; }
        #endregion

        public EmployeeEditViewModel()
        {
            using var db = new InventoryEntities();
            CollectionPosts = new List<Post>(db.Posts);
            CollectionDepartments = new List<Department>(db.Departments);
        }

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
            
            empEditWindow.Close();

        }, _ => IsValidation());

        public ICommand Cancel => new DelegateCommand<Window>(empEditWindow =>
        {
            
        });

        public ICommand AddPostInCollection => new DelegateCommand(() => PostsEmployees.Add(new Posts_employees()));

        public ICommand DeletePostFromCollection => new DelegateCommand<Posts_employees>((postEmp) => PostsEmployees.Remove(postEmp));

        public ICommand AddDepartmentInCollection => new DelegateCommand(() => EmployeesInDepartments.Add(new Employees_in_departments()));

        public ICommand DeleteDepartmentFromCollection => new DelegateCommand<Employees_in_departments>((empInDepart) => EmployeesInDepartments.Remove(empInDepart));
        #endregion
    }
}
