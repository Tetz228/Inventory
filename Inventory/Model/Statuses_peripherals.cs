namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;
    using DevExpress.Mvvm;

    using Inventory.ViewModels.Tables.Peripherals;

    public partial class Statuses_peripherals : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Statuses_peripherals()
        {
            this.Inventory_numbers_peripherals = new HashSet<Inventory_numbers_peripherals>();
        }
    
        public int Id_status_peripheral { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_peripherals> Inventory_numbers_peripherals { get; set; }

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
        public static bool Search(Statuses_peripherals statusPeripheral, string statusPeripheralFilter) => statusPeripheral.Name.ToLower().Contains(statusPeripheralFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddStatusPeripheral(string name)
        {
            using var db = new InventoryEntities();

            var statusPeripheral = new Statuses_peripherals()
            {
                Name = name
            };

            db.Statuses_peripherals.Add(statusPeripheral);
            db.SaveChanges();

            StatusesPeripheralsViewModel.StatusesPeripherals.Add(statusPeripheral);
        }

        public static void EditStatusPeripheral(Statuses_peripherals statusPeripheral)
        {
            using var db = new InventoryEntities();
            var findStatusPeripheral = db.Statuses_peripherals.FirstOrDefault(status => status.Id_status_peripheral == statusPeripheral.Id_status_peripheral);

            if (findStatusPeripheral == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении статуса периферии",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findStatusPeripheral.Name = statusPeripheral.Name;
            db.SaveChanges();
        }

        public static void DeleteStatusPeripheral(Statuses_peripherals selectStatusPeripheral)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectStatusPeripheral.Name}?",
                "Удаление статуса периферии", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findStatusPeripheral = db.Statuses_peripherals.FirstOrDefault(statusPeripheral => statusPeripheral.Id_status_peripheral == selectStatusPeripheral.Id_status_peripheral);

            if (findStatusPeripheral == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении статуса периферии", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Statuses_peripherals.Remove(findStatusPeripheral);
                db.SaveChanges();

                StatusesPeripheralsViewModel.StatusesPeripherals.Remove(selectStatusPeripheral);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить статус периферии, так как он связана с другими сущностями!",
                    "Ошибка при удалении статуса периферии", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            StatusesPeripheralsViewModel.StatusesPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Statuses_peripherals)
                StatusesPeripheralsViewModel.StatusesPeripherals.Add(item);
        }
        #endregion

        #region Откат изменений
        private Statuses_peripherals _selectStatusPeripheral;

        public void BeginEdit()
        {
            _selectStatusPeripheral = new Statuses_peripherals
            {
                Id_status_peripheral = this.Id_status_peripheral,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectStatusPeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectStatusPeripheral == null)
                return;

            Id_status_peripheral = _selectStatusPeripheral.Id_status_peripheral;
            Name = _selectStatusPeripheral.Name;
        }
        #endregion
    }
}
