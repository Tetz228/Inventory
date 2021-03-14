namespace Inventory.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables;

    public partial class Manufacturer : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Manufacturer()
        {
            this.Graphics_cards = new HashSet<Graphics_cards>();
            this.Motherboards = new HashSet<Motherboard>();
            this.Peripherals = new HashSet<Peripheral>();
            this.Power_supplies = new HashSet<Power_supplies>();
            this.Processors = new HashSet<Processor>();
            this.Rams = new HashSet<Ram>();
            this.Ssds = new HashSet<Ssd>();
        }
    
        public int Id_manufacturer { get; set; }
        public string Name { get; set; }
        
        public virtual ICollection<Graphics_cards> Graphics_cards { get; set; }
        public virtual ICollection<Motherboard> Motherboards { get; set; }
        public virtual ICollection<Peripheral> Peripherals { get; set; }
        public virtual ICollection<Power_supplies> Power_supplies { get; set; }
        public virtual ICollection<Processor> Processors { get; set; }
        public virtual ICollection<Ram> Rams { get; set; }
        public virtual ICollection<Ssd> Ssds { get; set; }

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
        public static bool Search(Manufacturer manufacturer, string manufacturerFilter) => manufacturer.Name.ToLower().Contains(manufacturerFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddManufacturer(string name)
        {
            using var db = new InventoryEntities();

            var manufacturer = new Manufacturer()
            {
                Name = name
            };

            db.Manufacturers.Add(manufacturer);
            db.SaveChanges();

            ManufacturersViewModel.Manufacturers.Add(manufacturer);
        }

        public static void EditManufacturer(Manufacturer manufacturer)
        {
            using var db = new InventoryEntities();
            var findManufacturer = db.Manufacturers.SingleOrDefault(manufact => manufact.Id_manufacturer == manufacturer.Id_manufacturer);

            if (findManufacturer == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении производителя",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findManufacturer.Name = manufacturer.Name;
            db.SaveChanges();
        }

        public static void DeleteManufacturer(Manufacturer selectManufacturer)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectManufacturer.Name}?",
                "Удаление производителя", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findManufacturer = db.Manufacturers.SingleOrDefault(manufacturer => manufacturer.Id_manufacturer == selectManufacturer.Id_manufacturer);

            if (findManufacturer == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении производителя", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Manufacturers.Remove(findManufacturer);
                db.SaveChanges();

                ManufacturersViewModel.Manufacturers.Remove(selectManufacturer);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить производителя, так как он связана с другими сущностями!",
                    "Ошибка при удалении производителя", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            ManufacturersViewModel.Manufacturers.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Manufacturers)
                ManufacturersViewModel.Manufacturers.Add(item);
        }
        #endregion

        #region Откат изменений
        private Manufacturer _selectManufacturer;

        public void BeginEdit()
        {
            _selectManufacturer = new Manufacturer
            {
                Id_manufacturer = this.Id_manufacturer,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectManufacturer = null;
        }

        public void CancelEdit()
        {
            if (_selectManufacturer == null)
                return;

            Id_manufacturer = _selectManufacturer.Id_manufacturer;
            Name = _selectManufacturer.Name;
        }
        #endregion
    }
}
