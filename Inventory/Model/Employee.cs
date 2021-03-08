namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;

    public partial class Employee : BindableBase, IEditableObject, IDataErrorInfo
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Accounts = new HashSet<Account>();
            this.Dispensing_computers = new HashSet<Dispensing_computers>();
            this.Dispensing_peripherals = new HashSet<Dispensing_peripherals>();
            this.Employees_in_departments = new HashSet<Employees_in_departments>();
            this.Posts_employees = new HashSet<Posts_employees>();
        }

        #region Свойства
        public int Id_employee { get; set; }
        public string L_name { get; set; }
        public string F_name { get; set; }
        public string M_name { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dispensing_computers> Dispensing_computers { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dispensing_peripherals> Dispensing_peripherals { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employees_in_departments> Employees_in_departments { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Posts_employees> Posts_employees { get; set; }

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
                        else
                            result = "";
                        break;
                    case "F_name":
                        if (string.IsNullOrWhiteSpace(F_name))
                            result = "Поле не должно быть пустым";
                        else if (F_name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        else
                            result = "";
                        break;
                    case "M_name":
                        if (!string.IsNullOrWhiteSpace(M_name))
                        {
                            if (M_name.Length < 2)
                                result = "Поле должно содержать минимум 2 символа";
                            else
                                result = "";
                        }
                        else
                            result = "";
                        break;
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                            result = "Поле не должно быть пустым";
                        else if (!IsValidationEmail(Email))
                            result = "Некорректная почта";
                        else
                            result = "";
                        break;
                    case "Phone_number":
                        if (string.IsNullOrWhiteSpace(Phone_number))
                            result = "Поле не должно быть пустым";
                        else if (Phone_number.Length < 10)
                            result = "Поле должно содержать минимум 10 символа";
                        else
                            result = "";
                        break;
                }

                if (ErrorCollection.ContainsKey(name))
                    ErrorCollection[name] = result;
                else if (result != null)
                    ErrorCollection.Add(name, result);

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        private bool IsValidationEmail(string email)
        {
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

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null || ErrorCollection.All(item => item.Value == ""));

        public bool IsValidationCollections()
        {
            if (PostsEmployees.Count == 0 || EmployeesInDepartments.Count == 0)
                return false;

            return PostsEmployees.All(item => item.Fk_post != 0) && EmployeesInDepartments.All(item => item.Fk_department != 0);
        }
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

            foreach (var post in PostsEmployees)
                post.Fk_employee = emp.Id_employee;

            db.Posts_employees.AddRange(PostsEmployees);
            db.SaveChanges();

            foreach (var department in EmployeesInDepartments)
                department.Fk_employee = emp.Id_employee;

            db.Employees_in_departments.AddRange(EmployeesInDepartments);
            db.SaveChanges();

            //Для отображения должностей и отделов, которые только что были добавлены, в таблице
            emp.Employees_in_departments = new List<Employees_in_departments>(db.Employees_in_departments.Include(dep => dep.Department).Where(empDep => empDep.Fk_employee == emp.Id_employee));
            emp.Posts_employees = new List<Posts_employees>(db.Posts_employees.Include(post => post.Post).Where(postEmp => postEmp.Fk_employee == emp.Id_employee));

            EmployeesViewModel.Employees.Add(emp);
        }

        public static void EditEmployee(Employee selectEmployee)
        {
            using var db = new InventoryEntities();
            var findEmployee = db.Employees.Include(employeePost => employeePost.Posts_employees
                                           .Select(post => post.Post))
                                           .Include(empDepart => empDepart.Employees_in_departments
                                           .Select(depart => depart.Department))
                                           .Include(account => account.Accounts
                                           .Select(role => role.User.Roles_users))
                                           .SingleOrDefault(employee => employee.Id_employee == selectEmployee.Id_employee);

            if (findEmployee == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении сотрудника",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Refresh();
                return;
            }

            findEmployee.L_name = selectEmployee.L_name;
            findEmployee.F_name = selectEmployee.F_name;
            findEmployee.M_name = selectEmployee.M_name;
            findEmployee.Phone_number = selectEmployee.Phone_number;
            findEmployee.Email = selectEmployee.Email;

            db.SaveChanges();

            AddInCollections(db, findEmployee.Id_employee);

            Refresh();
        }

        private static void AddInCollections(InventoryEntities db, int idEmployee)
        {
            foreach (var post in PostsEmployees)
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

            foreach (var department in EmployeesInDepartments)
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

        public static void DeleteEmployee(Employee selectEmployee)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectEmployee.L_name} {selectEmployee.F_name} {selectEmployee.M_name}?",
                "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findEmployee = db.Employees.SingleOrDefault(employee => employee.Id_employee == selectEmployee.Id_employee);

            if (findEmployee == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении сотрудника", MessageBoxButton.OK, MessageBoxImage.Error);
                Refresh();
                return;
            }

            db.Employees.Remove(findEmployee);
            db.SaveChanges();

            EmployeesViewModel.Employees.Remove(selectEmployee);
        }

        public static void DeletePostFromCollection(Posts_employees selectPostEmp)
        {
            if (selectPostEmp.Id_post_employee != 0)
            {
                using var db = new InventoryEntities();
                var findPostEmployee = db.Posts_employees.SingleOrDefault(postEmployee =>
                    postEmployee.Id_post_employee == selectPostEmp.Id_post_employee);

                if (findPostEmployee != null)
                {
                    db.Posts_employees.Remove(findPostEmployee);
                    db.SaveChanges();
                }
            }

            PostsEmployees.Remove(selectPostEmp);
        }

        public static void DeleteDepartmentFromCollection(Employees_in_departments selectEmpInDepart)
        {
            if (selectEmpInDepart.Id_employee_in_department != 0)
            {
                using var db = new InventoryEntities();
                var findEmpDepart = db.Employees_in_departments.SingleOrDefault(inDepartments => inDepartments.Id_employee_in_department == selectEmpInDepart.Id_employee_in_department);

                if (findEmpDepart != null)
                {
                    db.Employees_in_departments.Remove(findEmpDepart);
                    db.SaveChanges();
                }
            }

            EmployeesInDepartments.Remove(selectEmpInDepart);
        }

        public static void Refresh()
        {
            EmployeesViewModel.Employees.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Employees.Include(employeePost => employeePost.Posts_employees
                                                     .Select(post => post.Post))
                                                     .Include(empDepart => empDepart.Employees_in_departments
                                                     .Select(depart => depart.Department))
                                                     .Include(account => account.Accounts
                                                     .Select(role => role.User.Roles_users)))
            {
                EmployeesViewModel.Employees.Add(item);
            }
        }
        #endregion

        #region Откат изменений в случаи нажатия отмены
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