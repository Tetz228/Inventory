namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    using DevExpress.Mvvm;

    using Inventory.ViewModels.Tables.Employees;

    public partial class Role : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            this.Roles_users = new HashSet<Roles_users>();
        }

        #region Свойства
        public int Id_role { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Roles_users> Roles_users { get; set; }
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
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Name))
                            result = "Поле не должно быть пустым";
                        else if (Name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null);
        #endregion

        #region Метод поиска
        public static bool Search(Role role, string rolesFilter) => role.Name.ToLower().Contains(rolesFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddRole(string name)
        {
            using var db = new InventoryEntities();

            var role = new Role()
            {
                Name = name
            };

            db.Roles.Add(role);
            db.SaveChanges();

            RolesViewModel.Roles.Add(role);
        }

        public static void EditRole(Role selectRole)
        {
            using var db = new InventoryEntities();
            var findRole = db.Roles.SingleOrDefault(role => role.Id_role == selectRole.Id_role);

            if (findRole == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении роли",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findRole.Name = selectRole.Name;
            db.SaveChanges();
        }

        public static void DeleteRole(Role selectRole)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectRole.Name}?",
                "Удаление роли", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findRole = db.Roles.SingleOrDefault(role => role.Id_role == selectRole.Id_role);

            if (findRole == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении роли", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Roles.Remove(findRole);
                db.SaveChanges();

                RolesViewModel.Roles.Remove(selectRole);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить роль, так как она связана с другими сущностями!",
                    "Ошибка при удалении роли", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            RolesViewModel.Roles.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Roles)
                RolesViewModel.Roles.Add(item);
        }
        #endregion

        #region Откат изменений
        private Role _selectRole;

        public void BeginEdit()
        {
            _selectRole = new Role()
            {
                Id_role = this.Id_role,
                Name = this.Name,
            };
        }

        public void EndEdit()
        {
            _selectRole = null;
        }

        public void CancelEdit()
        {
            if (_selectRole == null)
                return;

            Id_role = _selectRole.Id_role;
            Name = _selectRole.Name;
        }
        #endregion
    }
}
