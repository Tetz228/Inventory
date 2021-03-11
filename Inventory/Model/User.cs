namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Employees;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;

    public partial class User : BindableBase, IEditableObject, IDataErrorInfo
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
                        break;
                    case "Password":
                        if (string.IsNullOrWhiteSpace(Password))
                            result = "Поле не должно быть пустым";
                        else if (Password.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.All(item => item.Value == null);

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
            using var db = new InventoryEntities();

            var newUser = new User
            {
                Login = user.Login,
                Password = user.Password,
                Fk_employee = user.Fk_employee
            };

            db.Users.Add(newUser);
            db.SaveChanges();

            UsersViewModel.Users.Add(newUser);
        }

        public static void EditUser(User selectUser)
        {
            using var db = new InventoryEntities();
            var findUser = db.Users.Include(employee => employee.Employee).SingleOrDefault(user => user.Id_user == selectUser.Id_user);

            if (findUser == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении пользователя",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findUser.Login = selectUser.Login;
            findUser.Password = selectUser.Password;
            findUser.Fk_employee = selectUser.Fk_employee;

            db.SaveChanges();

            RefreshCollection();
        }

        public static void DeleteUser(User selectUser)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectUser.Login}?",
                "Удаление пользователя", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findUser = db.Users.Include(employee => employee.Employee).SingleOrDefault(user => user.Id_user == selectUser.Id_user);

            if (findUser == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении пользователя", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            db.Users.Remove(findUser);
            db.SaveChanges();

            UsersViewModel.Users.Remove(selectUser);
        }

        public static void RefreshCollection()
        {
            UsersViewModel.Users.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Users.Include(employee => employee.Employee))
                UsersViewModel.Users.Add(item);
        }
        #endregion

        #region Откат изменений в случаи нажатия отмены
        private User _selectUser;

        public void BeginEdit()
        {
            _selectUser = new User
            {
                Id_user = this.Id_user,
                Login = this.Login,
                Password = this.Password,
                Fk_employee = this.Fk_employee
            };
        }

        public void EndEdit()
        {
            _selectUser = null;
        }

        public void CancelEdit()
        {
            if (_selectUser == null)
                return;

            Id_user = _selectUser.Id_user;
            Login = _selectUser.Login;
            Password = _selectUser.Password;
            Fk_employee = _selectUser.Fk_employee;
        }
        #endregion
    }
}