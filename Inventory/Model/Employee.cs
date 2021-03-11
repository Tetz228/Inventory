namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
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

        private bool IsValidationEmail(string email) => new EmailAddressAttribute().IsValid(email);

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

        public static bool Search(Employee employee, string employeesFilter)
        {
            return employee.L_name.ToLower().Contains(employeesFilter.ToLower()) || employee.F_name.ToLower().Contains(employeesFilter.ToLower()) ||
                   employee.Email.ToLower().Contains(employeesFilter.ToLower()) || employee.Phone_number.ToLower().Contains(employeesFilter.ToLower()) ||
                   ContainsCollectionPostsEmployees(employee, employeesFilter.ToLower()) || ContainsCollectionEmployeesInDepartments(employee, employeesFilter.ToLower());
        }

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

            Model.Posts_employees.AddPostEmployee(db, emp.Id_employee);
            Model.Employees_in_departments.AddEmployeeInDepartment(db, emp.Id_employee);

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
                                           .SingleOrDefault(employee => employee.Id_employee == selectEmployee.Id_employee);

            if (findEmployee == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении сотрудника",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findEmployee.L_name = selectEmployee.L_name;
            findEmployee.F_name = selectEmployee.F_name;
            findEmployee.M_name = selectEmployee.M_name;
            findEmployee.Phone_number = selectEmployee.Phone_number;
            findEmployee.Email = selectEmployee.Email;

            db.SaveChanges();

            Model.Posts_employees.EditPostEmployee(db, findEmployee.Id_employee);
            Model.Employees_in_departments.EditEmployeeInDepartment(db, findEmployee.Id_employee);

            RefreshCollection();
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
                RefreshCollection();
                return;
            }

            db.Employees.Remove(findEmployee);
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