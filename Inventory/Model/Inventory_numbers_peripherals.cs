namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    public partial class Inventory_numbers_peripherals : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_peripherals()
        {
            this.List_dispensed_peripherals = new HashSet<List_dispensed_peripherals>();
        }

        public int Id_inventory_number_peripheral { get; set; }
        public int Fk_peripheral { get; set; }
        public int Inventory_number { get; set; }
        public int Fk_status_peripheral { get; set; }

        public virtual Peripheral Peripheral { get; set; }
        public virtual Statuses_peripherals Statuses_peripherals { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<List_dispensed_peripherals> List_dispensed_peripherals { get; set; }

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Inventory_number":
                        if (Inventory_number <= 0)
                            result = "Поле должно содержать число больше нуля";
                        else
                        {
                            if (_selectInventoryNumberPeripheral == null)
                            {
                                if (IsUniqueInventoryNumber())
                                    result = "Номер должен быть уникальным";
                            }
                            else
                            {
                                if (_selectInventoryNumberPeripheral.Inventory_number != Inventory_number)
                                    if (IsUniqueInventoryNumber())
                                        result = "Номер должен быть уникальным";
                            }
                        }
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsUniqueInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isUniqueNumber = db.Inventory_numbers_peripherals.FirstOrDefault(number => number.Inventory_number == Inventory_number);

            return isUniqueNumber != null;
        }

        public static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_peripherals.FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_peripherals.Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 
                                                || ErrorCollection.Any(item => item.Value == null) 
                                                && Fk_peripheral != 0 && Fk_status_peripheral != 0;
        #endregion

        public static bool SearchFor(Inventory_numbers_peripherals inventoryNumberPeripheral, string inventoryNumberPeripheralFilter) => inventoryNumberPeripheral.Inventory_number.ToString().ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Peripheral.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Peripheral.Types_peripherals.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Peripheral.Manufacturer.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Statuses_peripherals.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower());

        #region Методы обработки информации
        public static void AddInventoryNumber(Inventory_numbers_peripherals inventoryNumber)
        {
            using var db = new InventoryEntities();

            var newInventoryNumberPeripheral = new Inventory_numbers_peripherals
            {
                Fk_peripheral = inventoryNumber.Fk_peripheral,
                Fk_status_peripheral = inventoryNumber.Fk_status_peripheral,
                Inventory_number = inventoryNumber.Inventory_number
            };

            db.Inventory_numbers_peripherals.Add(newInventoryNumberPeripheral);
            db.SaveChanges();

            newInventoryNumberPeripheral.Statuses_peripherals = db.Statuses_peripherals.FirstOrDefault(statusPeripheral => statusPeripheral.Id_status_peripheral == newInventoryNumberPeripheral.Fk_status_peripheral);
            newInventoryNumberPeripheral.Peripheral = db.Peripherals.FirstOrDefault(peripheral => peripheral.Id_peripheral == newInventoryNumberPeripheral.Fk_peripheral);

            if (newInventoryNumberPeripheral.Peripheral != null)
            {
                newInventoryNumberPeripheral.Peripheral.Manufacturer = db.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Id_manufacturer == newInventoryNumberPeripheral.Peripheral.Fk_manufacturer);
                newInventoryNumberPeripheral.Peripheral.Types_peripherals = db.Types_peripherals.FirstOrDefault(typePeripheral => typePeripheral.Id_type_peripheral == newInventoryNumberPeripheral.Peripheral.Fk_type_peripheral);
            }

            InventoryPeripheralsViewModel.InventoryNumbersPeripherals.Add(newInventoryNumberPeripheral);
        }

        public static void EditInventoryNumber(Inventory_numbers_peripherals selectInventoryNumber)
        {
            using var db = new InventoryEntities();
            var foundInventoryNumber = db.Inventory_numbers_peripherals.FirstOrDefault(inventoryNumberPeripheral => inventoryNumberPeripheral.Id_inventory_number_peripheral == selectInventoryNumber.Id_inventory_number_peripheral);

            if (foundInventoryNumber == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении инвентарного номера периферии",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            foundInventoryNumber.Fk_peripheral = selectInventoryNumber.Fk_peripheral;
            foundInventoryNumber.Fk_status_peripheral = selectInventoryNumber.Fk_status_peripheral;
            foundInventoryNumber.Inventory_number = selectInventoryNumber.Inventory_number;

            db.SaveChanges();

            RefreshCollection();
        }

        public static void DeleteInventoryNumberPeripheral(Inventory_numbers_peripherals selectInventoryNumber)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectInventoryNumber.Inventory_number}, {selectInventoryNumber.Peripheral.Name}?",
                "Удаление инвентарного номера периферии", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var foundInventoryNumber = db.Inventory_numbers_peripherals.FirstOrDefault(inventoryNumberPeripheral => inventoryNumberPeripheral.Id_inventory_number_peripheral == selectInventoryNumber.Id_inventory_number_peripheral);

            if (foundInventoryNumber == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при инвентарного номера периферии", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Inventory_numbers_peripherals.Remove(foundInventoryNumber);
                db.SaveChanges();

                InventoryPeripheralsViewModel.InventoryNumbersPeripherals.Remove(selectInventoryNumber);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить инвентарный номер периферии, так как он связана с другими сущностями!",
                    "Ошибка при удалении инвентарного номера периферии", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            InventoryPeripheralsViewModel.InventoryNumbersPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_peripherals
                                                                                        .Include(status => status.Statuses_peripherals)
                                                                                        .Include(peripheral => peripheral.Peripheral)
                                                                                        .Include(manufacturer => manufacturer.Peripheral.Manufacturer)
                                                                                        .Include(typePeripheral => typePeripheral.Peripheral.Types_peripherals))
            {
                InventoryPeripheralsViewModel.InventoryNumbersPeripherals.Add(item);
            }
        }
        #endregion

        #region Откат изменений
        private Inventory_numbers_peripherals _selectInventoryNumberPeripheral;

        public void BeginEdit()
        {
            _selectInventoryNumberPeripheral = new Inventory_numbers_peripherals()
            {
                Id_inventory_number_peripheral = this.Id_inventory_number_peripheral,
                Inventory_number = this.Inventory_number,
                Fk_status_peripheral = this.Fk_status_peripheral,
                Fk_peripheral = this.Fk_peripheral
            };
        }

        public void EndEdit()
        {
            _selectInventoryNumberPeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectInventoryNumberPeripheral == null)
                return;

            Id_inventory_number_peripheral = _selectInventoryNumberPeripheral.Id_inventory_number_peripheral;
            Inventory_number = _selectInventoryNumberPeripheral.Inventory_number;
            Fk_status_peripheral = _selectInventoryNumberPeripheral.Fk_status_peripheral;
            Fk_peripheral = _selectInventoryNumberPeripheral.Fk_peripheral;
        }
        #endregion
    }
}
