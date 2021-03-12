namespace Inventory.Model
{
    using BCrypt.Net;
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;

    public partial class User : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Dispensing_computers = new HashSet<Dispensing_computers>();
            this.Dispensing_peripherals = new HashSet<Dispensing_peripherals>();
        }

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
                        else if (UniqueLogin())
                            result = "Логин уже существует";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null);

        public bool ValidPassword() => Password?.Length > 2;

        public bool EqualsPasswords() => Password.Equals(PasswordRepeated);

        public bool UniqueLogin()
        {
            using var db = new InventoryEntities();
            var findUser = db.Users.FirstOrDefault(user => user.Login == Login);

            if (findUser == null)
                return false;
            else
                return true;
        }
        #endregion

        public static bool Search(User user, string usersFilter)
        {
            return user.Login.ToLower().Contains(usersFilter.ToLower()) ||
                   user.Employee.L_name.ToLower().Contains(usersFilter.ToLower()) ||
                   user.Employee.F_name.ToLower().Contains(usersFilter.ToLower()) ||
                   user.Employee.Email.ToLower().Contains(usersFilter.ToLower());
        }

        #region Методы обработки информации
        public static void AddUser(User user)
        {
            (string salt, string hash) = User.GenerateSaltAndHashingPassword(user.Password);

            user.Salt = salt;
            user.Password = hash;

            using var db = new InventoryEntities();

            var newUser = new User
            {
                Login = user.Login,
                Password = user.Password,
                Fk_employee = user.Fk_employee,
                Salt = user.Salt
            };

            newUser.Employee = db.Employees.SingleOrDefault(emp => emp.Id_employee == newUser.Fk_employee);
            db.Users.Add(newUser);
            db.SaveChanges();

            UsersViewModel.Users.Add(newUser);
        }

        public static void RefreshCollection()
        {
            UsersViewModel.Users.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Users.Include(employee => employee.Employee))
                UsersViewModel.Users.Add(item);
        }

        public static (string salt, string hash) GenerateSaltAndHashingPassword(string password)
        {
            var salt = BCrypt.GenerateSalt(4);
            var hash = BCrypt.HashPassword(password, salt);

            return (salt, hash);
        }
        #endregion
    }
}
