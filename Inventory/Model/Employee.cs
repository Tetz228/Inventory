namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
    using System.Windows;

    public partial class Employee : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Dispensing_computers = new HashSet<Dispensing_computers>();
            this.Dispensing_peripherals = new HashSet<Dispensing_peripherals>();
            this.Employees_in_departments = new HashSet<Employees_in_departments>();
            this.Posts_employees = new HashSet<Posts_employees>();
            this.Users = new HashSet<User>();
        }

        #region Свойства
        public int Id_employee { get; set; }
        public string L_name { get; set; }
        public string F_name { get; set; }
        public string M_name { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dispensing_computers> Dispensing_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dispensing_peripherals> Dispensing_peripherals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees_in_departments> Employees_in_departments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Posts_employees> Posts_employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }

        public static ObservableCollection<Posts_employees> PostsEmployees { get; set; } = new();

        public static ObservableCollection<Employees_in_departments> EmployeesInDepartments { get; set; } = new();
        #endregion

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "L_name":
                        if (string.IsNullOrWhiteSpace(L_name))
                            result = "Поле не должно быть пустым";
                        else if (L_name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                    case "F_name":
                        if (string.IsNullOrWhiteSpace(F_name))
                            result = "Поле не должно быть пустым";
                        else if (F_name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                    case "M_name":
                        if (!string.IsNullOrWhiteSpace(M_name))
                            if (M_name.Length < 2)
                                result = "Поле должно содержать минимум 2 символа";
                        break;
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                            result = "Поле не должно быть пустым";
                        else if (!IsValidationEmail(Email))
                            result = "Некорректная почта";
                        break;
                    case "Phone_number":
                        if (string.IsNullOrWhiteSpace(Phone_number))
                            result = "Поле не должно быть пустым";
                        else if (Phone_number.Length < 5)
                            result = "Поле должно содержать минимум 5 символа";
                        else if (!IsValidationPhoneNumber(Phone_number))
                            result = "Некорректный номер";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public static bool IsValidationEmail(string email) => new EmailAddressAttribute().IsValid(email);

        private bool IsValidationPhoneNumber(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null);

        public bool IsValidationCollections()
        {
            if (PostsEmployees.Count == 0 || EmployeesInDepartments.Count == 0)
                return false;

            return PostsEmployees.All(item => item.Fk_post != 0) && EmployeesInDepartments.All(item => item.Fk_department != 0);
        }
        #endregion

        #region Методы поиска
        public static bool SearchFor(Employee employee, string employeesFilter) => employee.L_name.ToLower().Contains(employeesFilter.ToLower())
                                                                                || employee.F_name.ToLower().Contains(employeesFilter.ToLower())
                                                                                || employee.Email.ToLower().Contains(employeesFilter.ToLower())
                                                                                || employee.Phone_number.ToLower().Contains(employeesFilter.ToLower())
                                                                                || ContainsCollectionPostsEmployees(employee, employeesFilter.ToLower())
                                                                                || ContainsCollectionEmployeesInDepartments(employee, employeesFilter.ToLower());

        public static bool ContainsCollectionPostsEmployees(Employee employee, string employeesFilter) => employee.Posts_employees.Select(postsEmployees => postsEmployees.Post.Name.ToLower().Contains(employeesFilter)).FirstOrDefault();

        public static bool ContainsCollectionEmployeesInDepartments(Employee employee, string employeesFilter) => employee.Employees_in_departments.Select(employeesInDepartments => employeesInDepartments.Department.Name.ToLower().Contains(employeesFilter)).FirstOrDefault();
        #endregion

        #region Методы обработки информации
        public static void AddEmployee(Employee employee)
        {
            using var db = new InventoryEntities();

            var emp = new Employee
            {
                L_name = employee.L_name,
                F_name = employee.F_name,
                M_name = employee.M_name,
                Phone_number = employee.Phone_number,
                Email = employee.Email
            };

            db.Employees.Add(emp);
            db.SaveChanges();

            
            //Model.Employees_in_departments.AddEmployeeInDepartment(db, emp.Id_employee);

            emp.Employees_in_departments = new List<Employees_in_departments>(db.Employees_in_departments.Include(dep => dep.Department).Where(empDep => empDep.Fk_employee == emp.Id_employee));
            emp.Posts_employees = new List<Posts_employees>(db.Posts_employees.Include(post => post.Post).Where(postEmp => postEmp.Fk_employee == emp.Id_employee));

            EmployeesViewModel.Employees.Add(emp);
        }

        public static void EditEmployee(Employee selectEmployee)
        {
            using var db = new InventoryEntities();
            var foundEmployee = db.Employees.Include(employeePost => employeePost.Posts_employees
                                           .Select(post => post.Post))
                                           .Include(empDepart => empDepart.Employees_in_departments
                                           .Select(depart => depart.Department))
                                           .FirstOrDefault(employee => employee.Id_employee == selectEmployee.Id_employee);

            if (foundEmployee == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении сотрудника",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            foundEmployee.L_name = selectEmployee.L_name;
            foundEmployee.F_name = selectEmployee.F_name;
            foundEmployee.M_name = selectEmployee.M_name;
            foundEmployee.Phone_number = selectEmployee.Phone_number;
            foundEmployee.Email = selectEmployee.Email;

            db.SaveChanges();

            //Model.Posts_employees.EditPostEmployee(db, foundEmployee.Id_employee);
            //Model.Employees_in_departments.EditEmployeeInDepartment(db, foundEmployee.Id_employee);

            RefreshCollection();
        }

        public static void DeleteEmployee(Employee selectEmployee)
        {
            using var db = new InventoryEntities();
            var foundEmployee = db.Employees.Include(post => post.Posts_employees)
                .Include(depart => depart.Employees_in_departments)
                .FirstOrDefault(employee => employee.Id_employee == selectEmployee.Id_employee);

            if (foundEmployee == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении сотрудника", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            var postEmp = db.Posts_employees.Where(emp => emp.Fk_employee == foundEmployee.Id_employee);
            var depEmp = db.Employees_in_departments.Where(emp => emp.Fk_employee == foundEmployee.Id_employee);
            db.Posts_employees.RemoveRange(postEmp);
            db.Employees_in_departments.RemoveRange(depEmp);
            //db.Employees.Remove(foundEmployee);
            db.SaveChanges();

            EmployeesViewModel.Employees.Remove(selectEmployee);
        }

        public static void RefreshCollection()
        {
            EmployeesViewModel.Employees.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Employees.Include(employeePost => employeePost.Posts_employees
                                                     .Select(post => post.Post))
                                                     .Include(empDepart => empDepart.Employees_in_departments
                                                     .Select(depart => depart.Department)))
            {
                EmployeesViewModel.Employees.Add(item);
            }
        }

        public static (Employee, bool) OnEmailExist(string email)
        {
            using var db = new InventoryEntities();
            var foundEmployee = db.Employees.FirstOrDefault(employee => employee.Email == email);

            if (foundEmployee == null)
            {
                MessageBox.Show("Сотрудник c такой почтой не найден! Проверьте правильность написания почты.", "Ошибка! Сотрудник не найден.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (null, false);
            }

            return (foundEmployee, true);
        }

        public static (int, bool) SendingSecurityCode(string email)
        {
            var random = new Random();
            var fromMailAddress = new MailAddress("itproject719@gmail.com", "ITProject");
            var toMailAddress = new MailAddress(email);
            int code = random.Next(1000, 9999);

            using var mailMessager = new MailMessage(fromMailAddress, toMailAddress)
            {
                Subject = "Восставноление пароля",
                Body = "Ваш код безопасности для восставноления пароля - " + code,
                IsBodyHtml = false
            };

            using var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMailAddress.Address, "%*kHy#l7~x")
            };

            try
            {
                smtp.Send(mailMessager);

                MessageBox.Show("Код безопасности отправлен на почту! Если сообщение с кодом не пришло, то посмотрите в папке спам.", "Код отправлен", MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return (code, true);
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка! Код безопасноcти не отправлен.", "Ошибка при отправке кода безопасности.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (0, false);
            }
        }
        #endregion

        #region Откат изменений
        private Employee _selectEmployee;

        public void BeginEdit()
        {
            _selectEmployee = new Employee
            {
                Id_employee = this.Id_employee,
                L_name = this.L_name,
                F_name = this.F_name,
                M_name = this.M_name,
                Phone_number = this.Phone_number,
                Email = this.Email
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

            Id_employee = _selectEmployee.Id_employee;
            L_name = _selectEmployee.L_name;
            F_name = _selectEmployee.F_name;
            M_name = _selectEmployee.M_name;
            Phone_number = _selectEmployee.Phone_number;
            Email = _selectEmployee.Email;
        }
        #endregion
    }
}
