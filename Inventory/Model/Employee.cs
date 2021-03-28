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

        public bool IsValidationCollections()
        {
            if (PostsEmployees.Count == 0 || EmployeesInDepartments.Count == 0)
                return false;

            return PostsEmployees.All(item => item.Fk_post != 0) && EmployeesInDepartments.All(item => item.Fk_department != 0);
        }
        #endregion

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
    }
}
