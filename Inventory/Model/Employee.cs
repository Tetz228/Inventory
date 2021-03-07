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
                    case "Email":
                        if (string.IsNullOrWhiteSpace(Email))
                            result = "Поле не должно быть пустым";
                        else if (!IsValidationEmail(Email))
                            result = "Некорректная почта";
                        break;
                    case "Phone_number":
                        if (string.IsNullOrWhiteSpace(Phone_number))
                            result = "Поле не должно быть пустым";
                        else if (Phone_number.Length < 10)
                            result = "Поле должно содержать минимум 10 символа";
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

        private static bool IsValidationEmail(string email)
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

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null);

        public bool IsValidationCollections()
        {
            if (PostsEmployees.Count == 0 || EmployeesInDepartments.Count == 0)
                return false;

            return PostsEmployees.All(item => item.Fk_post != 0) && EmployeesInDepartments.All(item => item.Fk_department != 0);
        }
        #endregion

        #region Методы взаимодействия с информацией
        public static Task<bool> AddEmployee(Employee employee)
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

            emp.Employees_in_departments = new List<Employees_in_departments>(db.Employees_in_departments.Include(dep => dep.Department).Where(empDep => empDep.Fk_employee == emp.Id_employee));
            emp.Posts_employees = new List<Posts_employees>(db.Posts_employees.Include(post => post.Post).Where(postEmp => postEmp.Fk_employee == emp.Id_employee));

            EmployeesViewModel.Employees.Add(emp);

            return Task.FromResult(true);
        }

        public static Task<bool> EditEmployee(Employee employee)
        {
            //using var db = new InventoryEntities();
            //var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == post.Id_department);

            //if (findDepartment == null)
            //{
            //    MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении отдела",
            //        MessageBoxButton.OK, MessageBoxImage.Error);
            //    return Task.FromResult(false);
            //}

            //findDepartment.Name = post.Name;
            //db.SaveChanges();
            return Task.FromResult(true);
        }

        public static Task<bool> DeleteEmployee(Employee employee)
        {
            //if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectDepartment.Name}?",
            //    "Удаление отдела", MessageBoxButton.YesNo, MessageBoxImage.Question))
            //    return Task.FromResult(false);

            //using var db = new InventoryEntities();
            //var findDepartment = db.Departments.SingleOrDefault(department => department.Id_department == selectDepartment.Id_department);

            //if (findDepartment == null)
            //{
            //    MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return Task.FromResult(false);
            //}

            //try
            //{
            //    db.Departments.Remove(findDepartment);
            //    db.SaveChanges();

            //    DepartmentViewModel.Departments.Remove(selectDepartment);
            //}
            //catch (DbUpdateException)
            //{
            //    MessageBox.Show("Невозможно удалить отдел, так как он связан с другими сущностями!",
            //        "Ошибка при удалении отдела", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return Task.FromResult(false);
            //}

            return Task.FromResult(true);
        }

        public static Task<bool> Refresh()
        {
            EmployeesViewModel.Employees.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Employees)
                EmployeesViewModel.Employees.Add(item);

            return Task.FromResult(true);
        }
        #endregion

        #region Откат изменений
        private Employee _selectEmployee;

        public void BeginEdit()
        {
            _selectEmployee = new Employee()
            {
                Id_employee = this.Id_employee,
                L_name = this.L_name,
                F_name = this.F_name,
                M_name = this.M_name,
                Email = this.Email,
                Phone_number = this.Phone_number,
                Posts_employees = this.Posts_employees,
                Employees_in_departments = this.Employees_in_departments
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

            //Id_department = _selectEmployee.Id_department;
            //Name = _selectEmployee.Name;
        }
        #endregion
    }
}