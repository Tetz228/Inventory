namespace Inventory.Model
{
    using System;
    using BCrypt.Net;
    using System.Collections.Generic;
    using System.ComponentModel;
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System.Linq;
    using System.Windows;
    using System.Data.Entity;

    using Inventory.Model.Classes;

    public partial class User : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Dispensing_computers = new HashSet<Dispensing_computers>();
            Dispensing_peripherals = new HashSet<Dispensing_peripherals>();
        }

        public static int AuthorizedUser { get; set; }

        public int Id_user { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Fk_employee { get; set; }
        public string Salt { get; set; }
        public string PasswordRepeated { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dispensing_computers> Dispensing_computers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dispensing_peripherals> Dispensing_peripherals { get; set; }
        public virtual Employee Employee { get; set; }

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Login":
                        if (string.IsNullOrWhiteSpace(Login))
                            result = "Поле не должно быть пустым";
                        else if (Login.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        else if (UsersInteraction.UniqueLogin(Login))
                            result = "Логин уже существует";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }
        #endregion
    }
}
