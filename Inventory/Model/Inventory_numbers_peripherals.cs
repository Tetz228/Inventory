namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.Generic;
    using System.ComponentModel;
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
                    case "Name":
                        if (string.IsNullOrWhiteSpace(Inventory_number.ToString()))
                            result = "Поле не должно быть пустым";
                        else if (Inventory_number.ToString().Length <= 1)
                            result = "Поле должно содержать минимум 1 символа";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null) && Fk_peripheral != 0 && Fk_status_peripheral != 0;
        #endregion

        #region Метод поиска
        public static bool Search(Inventory_numbers_peripherals inventoryNumberPeripheral, string inventoryNumberPeripheralFilter) => inventoryNumberPeripheral.Inventory_number.ToString().ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Peripheral.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Peripheral.Types_peripherals.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Peripheral.Manufacturer.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower())
            || inventoryNumberPeripheral.Statuses_peripherals.Name.ToLower().Contains(inventoryNumberPeripheralFilter.ToLower());
        #endregion

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

            //newInventoryNumberPeripheral.Statuses_peripherals = db.Statuses_peripherals.SingleOrDefault(statusPeripheral => statusPeripheral.Id_status_peripheral == newInventoryNumberPeripheral.Fk_status_peripheral);
            //newInventoryNumberPeripheral.Peripheral = db.Peripherals.SingleOrDefault(typesPeripherals => typesPeripherals.Id_type_peripheral == newInventoryNumberPeripheral.Fk_type_peripheral);

            InventoryPeripheralsViewModel.InventoryNumbersPeripherals.Add(newInventoryNumberPeripheral);
        }

        public static void EditInventoryNumber(Inventory_numbers_peripherals selectInventoryNumber)
        {
            using var db = new InventoryEntities();
            var findInventoryNumber = db.Inventory_numbers_peripherals.SingleOrDefault(inventoryNumberPeripheral => inventoryNumberPeripheral.Id_inventory_number_peripheral == selectInventoryNumber.Id_inventory_number_peripheral);

            if (findInventoryNumber == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении инвентарного номера периферии",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findInventoryNumber.Fk_peripheral = selectInventoryNumber.Fk_peripheral;
            findInventoryNumber.Fk_status_peripheral = selectInventoryNumber.Fk_status_peripheral;
            findInventoryNumber.Inventory_number = selectInventoryNumber.Inventory_number;

            db.SaveChanges();

            RefreshCollection();
        }

        public static void DeletePeripheral(Inventory_numbers_peripherals selectInventoryNumber)
        {
            //if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectInventoryNumber.Name}?",
            //    "Удаление периферии", MessageBoxButton.YesNo, MessageBoxImage.Question))
            //    return;

            //using var db = new InventoryEntities();
            //var findPeripheral = db.Peripherals.SingleOrDefault(peripheral => peripheral.Id_peripheral == selectInventoryNumber.Id_peripheral);

            //if (findPeripheral == null)
            //{
            //    MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении периферии", MessageBoxButton.OK, MessageBoxImage.Error);
            //    RefreshCollection();
            //    return;
            //}

            //try
            //{
            //    db.Peripherals.Remove(findPeripheral);
            //    db.SaveChanges();

            //    PeripheralsViewModel.Peripherals.Remove(selectInventoryNumber);
            //}
            //catch (DbUpdateException)
            //{
            //    MessageBox.Show("Невозможно удалить тип периферии, так как он связана с другими сущностями!",
            //        "Ошибка при удалении периферии", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        public static void RefreshCollection()
        {
            //InventoryPeripheralsViewModel.InventoryNumbersPeripherals.Clear();
            //using var db = new InventoryEntities();

            //foreach (var item in db.Inventory_numbers_peripherals.Include(manufacturer => manufacturer.Manufacturer).Include(typePeripheral => typePeripheral.Types_peripherals))
            //    InventoryPeripheralsViewModel.InventoryNumbersPeripherals.Add(item);
        }
        #endregion

        #region Откат изменений
        private Inventory_numbers_peripherals _selectPeripheral;

        public void BeginEdit()
        {
            _selectPeripheral = new Inventory_numbers_peripherals()
            {
                //Id_peripheral = this.Id_peripheral,
                //Fk_manufacturer = this.Fk_manufacturer,
                //Fk_type_peripheral = this.Fk_type_peripheral,
                //Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectPeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectPeripheral == null)
                return;

            //Id_peripheral = _selectPeripheral.Id_peripheral;
            //Fk_manufacturer = _selectPeripheral.Fk_manufacturer;
            //Fk_type_peripheral = _selectPeripheral.Fk_type_peripheral;
            //Name = _selectPeripheral.Name;
        }
        #endregion
    }
}
