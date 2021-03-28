namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.Model.Classes;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text.RegularExpressions;

    public partial class Employee : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            Dispensing_computers = new HashSet<Dispensing_computers>();
            Dispensing_peripherals = new HashSet<Dispensing_peripherals>();
            Employees_in_departments = new HashSet<Employees_in_departments>();
            Posts_employees = new HashSet<Posts_employees>();
            Users = new HashSet<User>();
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
                        else if (MailsInteraction.IsValidationEmail(Email) == false)
                            result = "Некорректная почта";
                        break;
                    case "Phone_number":
                        if (string.IsNullOrWhiteSpace(Phone_number))
                            result = "Поле не должно быть пустым";
                        else if (Phone_number.Length < 5)
                            result = "Поле должно содержать минимум 5 символа";
                        else if (IsValidationPhoneNumber(Phone_number) == false)
                            result = "Некорректный номер";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        private bool IsValidationPhoneNumber(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$");

        public bool IsValidationCollections()
        {
            if (PostsEmployees.Count == 0 || EmployeesInDepartments.Count == 0)
                return false;

            return PostsEmployees.All(item => item.Fk_post != 0) && EmployeesInDepartments.All(item => item.Fk_department != 0);
        }
        #endregion
    }
}
