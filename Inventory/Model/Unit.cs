namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Other;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

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

        public static bool SearchFor(Unit unit, string unitsFilter) => unit.Full_name.ToLower().Contains(unitsFilter.ToLower()) || unit.Short_name.ToLower().Contains(unitsFilter.ToLower());

        public static void RefreshCollection()
        {
            UnitsViewModel.Units.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Units)
                UnitsViewModel.Units.Add(item);
        }

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