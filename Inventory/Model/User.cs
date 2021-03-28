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
            var foundUser = db.Users.FirstOrDefault(user => user.Login == Login);

            if (foundUser == null)
                return false;
            else
                return true;
        }
        #endregion

        #region Методы обработки информации
        public static void ChangePassword(User user)
        {
            (string salt, string hash) = GenerateSaltAndHashingPassword(user.Password);

            user.Salt = salt;
            user.Password = hash;

            using var db = new InventoryEntities();

            var foundUser = db.Users.FirstOrDefault(u => u.Id_user == user.Id_user);

            if (foundUser == null)
            {
                MessageBox.Show("Объект не найден в базе данных! Пароль не был изменен.", "Ошибка при изменение пароля", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foundUser.Password = user.Password;
            foundUser.Salt = user.Salt;

            db.SaveChanges();

            MessageBox.Show("Пароль успешно изменен!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static (string salt, string hash) GenerateSaltAndHashingPassword(string password)
        {
            var salt = BCrypt.GenerateSalt(4);
            var hash = BCrypt.HashPassword(password, salt);

            return (salt, hash);
        }

        public static (int, bool) OnUserExist(string login, string password)
        {
            using var db = new InventoryEntities();
            var foundUser = db.Users.FirstOrDefault(user => user.Login == login);

            if (foundUser == null)
            {
                MessageBox.Show("Пользователь не найден! Проверьте правильность написания логина.", "Ошибка! Пользователь не найден.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (0, false);
            }

            if (BCrypt.Verify(password, foundUser.Password) == false)
            {
                MessageBox.Show("Неверный пароль! Проверьте правильность написания пароля.", "Ошибка! Неверный пароль.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (0, false);
            }

            return (foundUser.Id_user, true);
        }

        public static (int, bool) OnUserExist(Employee employee)
        {
            using var db = new InventoryEntities();
            var foundUser = db.Users.FirstOrDefault(user => user.Fk_employee == employee.Id_employee);

            if (foundUser == null)
            {
                MessageBox.Show("Пользователь c такой почтой не найден!", "Ошибка! Пользователь не найден.", MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return (0, false);
            }

            return (foundUser.Id_user, true);
        }
        #endregion
    }
}
