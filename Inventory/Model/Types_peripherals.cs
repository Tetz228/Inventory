namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    public partial class Types_peripherals : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Types_peripherals()
        {
            this.Peripherals = new HashSet<Peripheral>();
        }

        public int Id_type_peripheral { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Peripheral> Peripherals { get; set; }

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
        public static bool Search(Types_peripherals typePeripheral, string typePeripheralFilter) => typePeripheral.Name.ToLower().Contains(typePeripheralFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddTypePeripheral(string name)
        {
            using var db = new InventoryEntities();

            var typePeripheral = new Types_peripherals()
            {
                Name = name
            };

            db.Types_peripherals.Add(typePeripheral);
            db.SaveChanges();

            TypesPeripheralsViewModel.TypesPeripherals.Add(typePeripheral);
        }

        public static void EditTypePeripheral(Types_peripherals typePeripheral)
        {
            using var db = new InventoryEntities();
            var findTypePeripheral = db.Types_peripherals.FirstOrDefault(type => type.Id_type_peripheral == typePeripheral.Id_type_peripheral);

            if (findTypePeripheral == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении типа периферии",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findTypePeripheral.Name = typePeripheral.Name;
            db.SaveChanges();
        }

        public static void DeleteTypePeripheral(Types_peripherals selectTypePeripheral)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectTypePeripheral.Name}?",
                "Удаление типа периферии", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findTypePeripheral = db.Types_peripherals.FirstOrDefault(types => types.Id_type_peripheral == selectTypePeripheral.Id_type_peripheral);

            if (findTypePeripheral == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении типа периферии", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Types_peripherals.Remove(findTypePeripheral);
                db.SaveChanges();

                TypesPeripheralsViewModel.TypesPeripherals.Remove(selectTypePeripheral);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить тип периферии, так как он связана с другими сущностями!",
                    "Ошибка при удалении типа периферии", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            TypesPeripheralsViewModel.TypesPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_peripherals)
                TypesPeripheralsViewModel.TypesPeripherals.Add(item);
        }
        #endregion

        #region Откат изменений
        private Types_peripherals _selectTypePeripheral;

        public void BeginEdit()
        {
            _selectTypePeripheral = new Types_peripherals
            {
                Id_type_peripheral = this.Id_type_peripheral,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectTypePeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectTypePeripheral == null)
                return;

            Id_type_peripheral = _selectTypePeripheral.Id_type_peripheral;
            Name = _selectTypePeripheral.Name;
        }
        #endregion
    }
}