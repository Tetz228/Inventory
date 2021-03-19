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

    public partial class Peripheral : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Peripheral()
        {
            this.Inventory_numbers_peripherals = new HashSet<Inventory_numbers_peripherals>();
        }

        public int Id_peripheral { get; set; }
        public int Fk_type_peripheral { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Inventory_numbers_peripherals> Inventory_numbers_peripherals { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Types_peripherals Types_peripherals { get; set; }

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

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null) && Fk_manufacturer != 0 && Fk_type_peripheral != 0;
        #endregion

        #region Метод поиска
        public static bool Search(Peripheral peripheral, string peripheralFilter) => peripheral.Name.ToLower().Contains(peripheralFilter.ToLower())
            || peripheral.Types_peripherals.Name.ToLower().Contains(peripheralFilter.ToLower())
            || peripheral.Manufacturer.Name.ToLower().Contains(peripheralFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddPeripheral(Peripheral peripheral)
        {
            using var db = new InventoryEntities();

            var newPeripheral = new Peripheral()
            {
                Name = peripheral.Name,
                Fk_type_peripheral = peripheral.Fk_type_peripheral,
                Fk_manufacturer = peripheral.Fk_manufacturer
            };

            db.Peripherals.Add(newPeripheral);
            db.SaveChanges();

            newPeripheral.Manufacturer = db.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Id_manufacturer == newPeripheral.Fk_manufacturer);
            newPeripheral.Types_peripherals = db.Types_peripherals.FirstOrDefault(typesPeripherals => typesPeripherals.Id_type_peripheral == newPeripheral.Fk_type_peripheral);

            PeripheralsViewModel.Peripherals.Add(newPeripheral);
        }

        public static void EditPeripheral(Peripheral selectPeripheral)
        {
            using var db = new InventoryEntities();
            var findPeripheral = db.Peripherals.FirstOrDefault(peripheral => peripheral.Id_peripheral == selectPeripheral.Id_peripheral);

            if (findPeripheral == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении периферии",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findPeripheral.Name = selectPeripheral.Name;
            findPeripheral.Fk_manufacturer = selectPeripheral.Fk_manufacturer;
            findPeripheral.Fk_type_peripheral = selectPeripheral.Fk_type_peripheral;

            db.SaveChanges();

            RefreshCollection();
        }

        public static void DeletePeripheral(Peripheral selectPeripheral)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectPeripheral.Name}?",
                "Удаление периферии", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findPeripheral = db.Peripherals.FirstOrDefault(peripheral => peripheral.Id_peripheral == selectPeripheral.Id_peripheral);

            if (findPeripheral == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении периферии", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Peripherals.Remove(findPeripheral);
                db.SaveChanges();

                PeripheralsViewModel.Peripherals.Remove(selectPeripheral);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить тип периферии, так как он связана с другими сущностями!",
                    "Ошибка при удалении периферии", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            PeripheralsViewModel.Peripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Peripherals.Include(manufacturer => manufacturer.Manufacturer).Include(typePeripheral => typePeripheral.Types_peripherals))
                PeripheralsViewModel.Peripherals.Add(item);
        }
        #endregion

        #region Откат изменений
        private Peripheral _selectPeripheral;

        public void BeginEdit()
        {
            _selectPeripheral = new Peripheral()
            {
                Id_peripheral = this.Id_peripheral,
                Fk_manufacturer = this.Fk_manufacturer,
                Fk_type_peripheral = this.Fk_type_peripheral,
                Name = this.Name
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

            Id_peripheral = _selectPeripheral.Id_peripheral;
            Fk_manufacturer = _selectPeripheral.Fk_manufacturer;
            Fk_type_peripheral = _selectPeripheral.Fk_type_peripheral;
            Name = _selectPeripheral.Name;
        }
        #endregion
    }
}