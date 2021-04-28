namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public partial class Employee : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Dispensing_computers = new ObservableCollection<Dispensing_computers>();
            Dispensing_peripherals = new ObservableCollection<Dispensing_peripherals>();
            Employees_in_departments = new ObservableCollection<Employees_in_departments>();
            Posts_employees = new ObservableCollection<Posts_employees>();
            Users = new ObservableCollection<User>();
        }

        #region Свойства
        public int Id_employee { get; set; }
        public string L_name { get; set; }
        public string F_name { get; set; }
        public string M_name { get; set; }
        public string Email { get; set; }
        public string Phone_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Dispensing_computers> Dispensing_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Dispensing_peripherals> Dispensing_peripherals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Employees_in_departments> Employees_in_departments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<Posts_employees> Posts_employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ObservableCollection<User> Users { get; set; }
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
                        else if (MailsInteraction.IsValidationEmail(Email) == false)
                            result = "Некорректная почта";
                        else if (Services.CheckForUniqueness<Employee>(nameof(Email), Email, _selectEmployee?.Email))
                            result = "Сотрудник с такой почтой уже существует";
                        break;
                    case "Phone_number":
                        if (string.IsNullOrWhiteSpace(Phone_number))
                            result = "Поле не должно быть пустым";
                        else if (Phone_number.Length < 5)
                            result = "Поле должно содержать минимум 5 символа";
                        else if (IsValidationPhoneNumber(Phone_number) == false)
                            result = "Некорректный номер";
                        else if (Services.CheckForUniqueness<Employee>(nameof(Phone_number), Phone_number, _selectEmployee?.Phone_number))
                            result = "Сотрудник с таким номером уже существует";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        private bool IsValidationPhoneNumber(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");

        #endregion

        #region Откат изменений
        private Employee _selectEmployee;

        public void BeginEdit()
        {
            _selectEmployee = new Employee
            {
                Id_employee = Id_employee,
                L_name = L_name,
                F_name = F_name,
                M_name = M_name,
                Phone_number = Phone_number,
                Email = Email,
                Posts_employees = Posts_employees.CopyingElements(),
                Employees_in_departments = Employees_in_departments.CopyingElements()
            };
        }

        public void EndEdit() => _selectEmployee = null;

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
            Posts_employees = _selectEmployee.Posts_employees;
            Employees_in_departments = _selectEmployee.Employees_in_departments;
        }
        #endregion
    }
}
