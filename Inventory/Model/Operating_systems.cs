namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    using DevExpress.Mvvm;

    using Inventory.ViewModels.Tables.Computers;

    public partial class Operating_systems : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Operating_systems()
        {
            this.Operating_systems_in_computers = new HashSet<Operating_systems_in_computers>();
        }
    
        public int Id_operating_system { get; set; }
        public string Name { get; set; }
        public string System_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operating_systems_in_computers> Operating_systems_in_computers { get; set; }

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
                    case "System_version":
                        if (string.IsNullOrWhiteSpace(System_version))
                            result = "Поле не должно быть пустым";
                        else if (System_version.Length < 2)
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

        public static bool SearchFor(Operating_systems operatingSystem, string operatingSystemsFilter) => operatingSystem.Name.ToLower().Contains(operatingSystemsFilter.ToLower()) || operatingSystem.System_version.ToLower().Contains(operatingSystemsFilter.ToLower());

        #region Методы обработки информации
        public static void AddOperatingSystem(string name, string version)
        {
            using var db = new InventoryEntities();

            var operatingSystem = new Operating_systems()
            {
                Name = name,
                System_version = version
            };

            db.Operating_systems.Add(operatingSystem);
            db.SaveChanges();

            OperatingSystemsViewModel.OperatingSystems.Add(operatingSystem);
        }

        public static void EditOperatingSystem(Operating_systems operatingSystem)
        {
            using var db = new InventoryEntities();
            var foundSystem = db.Operating_systems.FirstOrDefault(system => system.Id_operating_system == operatingSystem.Id_operating_system);

            if (foundSystem == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении операционной системы",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            foundSystem.Name = operatingSystem.Name;
            foundSystem.System_version = operatingSystem.System_version;

            db.SaveChanges();
        }

        public static void DeleteOperatingSystem(Operating_systems selectOperatingSystem)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectOperatingSystem.Name}, версии {selectOperatingSystem.System_version}?",
                "Удаление операционной системы", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();

            var foundSystem = db.Operating_systems.FirstOrDefault(system => system.Id_operating_system == selectOperatingSystem.Id_operating_system);

            if (foundSystem == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении операционной системы", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Operating_systems.Remove(foundSystem);
                db.SaveChanges();

                OperatingSystemsViewModel.OperatingSystems.Remove(selectOperatingSystem);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить операционную систему, так как он связана с другими сущностями!",
                    "Ошибка при удалении операционной системы", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            OperatingSystemsViewModel.OperatingSystems.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Operating_systems)
                OperatingSystemsViewModel.OperatingSystems.Add(item);
        }
        #endregion

        #region Откат изменений
        private Operating_systems _selectOperatingSystem;

        public void BeginEdit()
        {
            _selectOperatingSystem = new Operating_systems
            {
                Id_operating_system = this.Id_operating_system,
                Name = this.Name,
                System_version = this.System_version
            };
        }

        public void EndEdit()
        {
            _selectOperatingSystem = null;
        }

        public void CancelEdit()
        {
            if (_selectOperatingSystem == null)
                return;

            Id_operating_system = _selectOperatingSystem.Id_operating_system;
            Name = _selectOperatingSystem.Name;
            System_version = _selectOperatingSystem.System_version;
        }
        #endregion
    }
}
