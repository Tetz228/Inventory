namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;

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

        public string Error { get => null; }

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

        public static void RefreshCollection()
        {
            HddsViewModel.Hdds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Hdds.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_hdd))
                HddsViewModel.Hdds.Add(item);
        }

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
