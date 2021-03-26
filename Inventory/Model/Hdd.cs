namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    public partial class Hdd : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hdd()
        {
            this.Inventory_numbers_hdd = new HashSet<Inventory_numbers_hdd>();
        }

        public int Id_hdd { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public double Memory_size { get; set; }
        public int Fk_unit { get; set; }
        public int Fk_type_hdd { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Types_hdd Types_hdd { get; set; }
        public virtual Unit Unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
            "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_hdd> Inventory_numbers_hdd { get; set; }

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
                    case "Memory_size":
                        if (Memory_size <= 0)
                            result = "Поле должно быть больше 0";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error
        {
            get => null;
        }

        public bool IsValidationProperties() => ErrorCollection.Count == 0
                                                || ErrorCollection.Any(item => item.Value == null)
                                                && Fk_manufacturer != 0
                                                && Fk_unit != 0
                                                && Fk_type_hdd != 0;

        #endregion

        public static bool SearchFor(Hdd hdd, string hddFilter) => hdd.Name.ToLower().Contains(hddFilter.ToLower())
                                                                || hdd.Unit.Full_name.ToLower().Contains(hddFilter.ToLower())
                                                                || hdd.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(hddFilter.ToLower())
                                                                || hdd.Unit.Short_name.ToLower().Contains(hddFilter.ToLower())
                                                                || hdd.Types_hdd.Name.ToLower().Contains(hddFilter.ToLower())
                                                                || hdd.Manufacturer.Name.ToLower().Contains(hddFilter.ToLower());

        #region Методы обработки информации
        public static void AddHdd(Hdd hdd)
        {
            using var db = new InventoryEntities();

            var newHdd = new Hdd()
            {
                Memory_size = hdd.Memory_size,
                Name = hdd.Name,
                Fk_type_hdd = hdd.Fk_type_hdd,
                Fk_unit = hdd.Fk_unit,
                Fk_manufacturer = hdd.Fk_manufacturer
            };

            db.Hdds.Add(newHdd);
            db.SaveChanges();

            newHdd.Manufacturer =
                db.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Id_manufacturer == newHdd.Fk_manufacturer);
            newHdd.Types_hdd = db.Types_hdd.FirstOrDefault(typesHdd => typesHdd.Id_type_hdd == newHdd.Fk_type_hdd);
            newHdd.Unit = db.Units.FirstOrDefault(unit => unit.Id_unit == newHdd.Fk_unit);

            HddsViewModel.Hdds.Add(newHdd);
        }

        public static void EditHdd(Hdd selectHdd)
        {
            using var db = new InventoryEntities();
            var foundHdd = db.Hdds.FirstOrDefault(hdd => hdd.Id_hdd == selectHdd.Id_hdd);

            if (foundHdd == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении жесткого диска",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            foundHdd.Memory_size = selectHdd.Memory_size;
            foundHdd.Name = selectHdd.Name;
            foundHdd.Fk_type_hdd = selectHdd.Fk_type_hdd;
            foundHdd.Fk_unit = selectHdd.Fk_unit;
            foundHdd.Fk_manufacturer = selectHdd.Fk_manufacturer;

            db.SaveChanges();

            RefreshCollection();
        }

        public static void DeleteHdd(Hdd selectHdd)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectHdd.Name}?",
                "Удаление жесткого диска", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var foundHdd = db.Hdds.FirstOrDefault(hdd => hdd == selectHdd);

            if (foundHdd == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении жесткого диска",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Hdds.Remove(foundHdd);
                db.SaveChanges();

                HddsViewModel.Hdds.Remove(selectHdd);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить жесткий диск, так как он связана с другими сущностями!",
                    "Ошибка при удалении жесткого диска", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            HddsViewModel.Hdds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Hdds.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_hdd))
                HddsViewModel.Hdds.Add(item);
        }
        #endregion

        #region Откат изменений
        private Hdd _selectHdd;

        public void BeginEdit()
        {
            _selectHdd = new Hdd()
            {
                Id_hdd = this.Id_hdd,
                Memory_size = this.Memory_size,
                Name = this.Name,
                Fk_type_hdd = this.Fk_type_hdd,
                Fk_unit = this.Fk_unit,
                Fk_manufacturer = this.Fk_manufacturer,
            };
        }

        public void EndEdit()
        {
            _selectHdd = null;
        }

        public void CancelEdit()
        {
            if (_selectHdd == null)
                return;
            Id_hdd = _selectHdd.Id_hdd;
            Memory_size = _selectHdd.Memory_size;
            Name = _selectHdd.Name;
            Fk_type_hdd = _selectHdd.Fk_type_hdd;
            Fk_unit = _selectHdd.Fk_unit;
            Fk_manufacturer = _selectHdd.Fk_manufacturer;
        }
        #endregion
    }
}
