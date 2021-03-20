namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    public partial class Unit : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unit()
        {
            this.Graphics_cards = new HashSet<Graphics_cards>();
            this.Hdds = new HashSet<Hdd>();
            this.Power_supplies = new HashSet<Power_supplies>();
            this.Processors = new HashSet<Processor>();
            this.Rams = new HashSet<Ram>();
            this.Ssds = new HashSet<Ssd>();
        }
    
        public int Id_unit { get; set; }
        public string Full_name { get; set; }
        public string Short_name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Graphics_cards> Graphics_cards { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hdd> Hdds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Power_supplies> Power_supplies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Processor> Processors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ram> Rams { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
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
                    case "Full_name":
                        if (string.IsNullOrWhiteSpace(Full_name))
                            result = "Поле не должно быть пустым";
                        else if (Full_name.Length < 2)
                            result = "Поле должно содержать минимум 2 символа";
                        break;
                    case "Short_name":
                        if (string.IsNullOrWhiteSpace(Short_name))
                            result = "Поле не должно быть пустым";
                        else if (Short_name.Length < 2)
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
        public static bool Search(Unit unit, string unitsFilter) => unit.Full_name.ToLower().Contains(unitsFilter.ToLower())|| unit.Short_name.ToLower().Contains(unitsFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddUnit(string fullName, string shortName)
        {
            using var db = new InventoryEntities();

            var unit = new Unit()
            {
                Full_name = fullName,
                Short_name = shortName
            };

            db.Units.Add(unit);
            db.SaveChanges();

            UnitsViewModel.Units.Add(unit);
        }

        public static void EditUnit(Unit unit)
        {
            using var db = new InventoryEntities();
            var findUnit = db.Units.FirstOrDefault(u => u.Id_unit == unit.Id_unit);

            if (findUnit == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении единицы измерения",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findUnit.Full_name = unit.Full_name;
            findUnit.Short_name = unit.Short_name;

            db.SaveChanges();
        }

        public static void DeleteUnit(Unit selectUnit)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectUnit.Full_name}?",
                "Удаление типа периферии", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findUnit = db.Units.FirstOrDefault(u => u.Id_unit == selectUnit.Id_unit);

            if (findUnit == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении единицы измерения", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Units.Remove(findUnit);
                db.SaveChanges();

                UnitsViewModel.Units.Remove(selectUnit);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить единицу измерения, так как он связана с другими сущностями!",
                    "Ошибка при удалении единицы измерения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            UnitsViewModel.Units.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Units)
                UnitsViewModel.Units.Add(item);
        }
        #endregion

        #region Откат изменений
        private Unit _selectUnit;

        public void BeginEdit()
        {
            _selectUnit = new Unit
            {
                Id_unit = this.Id_unit,
                Full_name = this.Full_name,
                Short_name = this.Short_name
            };
        }

        public void EndEdit()
        {
            _selectUnit = null;
        }

        public void CancelEdit()
        {
            if (_selectUnit == null)
                return;

            Id_unit = _selectUnit.Id_unit;
            Full_name = _selectUnit.Full_name;
            Short_name = _selectUnit.Short_name;
        }
        #endregion
    }
}
