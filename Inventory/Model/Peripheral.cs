namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Linq;

    public partial class Peripheral : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Peripheral()
        {
            this.Inventory_numbers_peripherals = new HashSet<Inventory_numbers_peripherals>();
        }

        public int Id_peripheral { get; set; }
        public int Fk_type_peripheral { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
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

        public bool IsValidationProperties() => ErrorCollection.Count == 0
                                                || ErrorCollection.Any(item => item.Value == null)
                                                && Fk_manufacturer != 0
                                                && Fk_type_peripheral != 0;
        #endregion

        public static bool SearchFor(Peripheral peripheral, string peripheralFilter) => peripheral.Name.ToLower().Contains(peripheralFilter.ToLower())
                                                                                     || peripheral.Types_peripherals.Name.ToLower().Contains(peripheralFilter.ToLower())
                                                                                     || peripheral.Manufacturer.Name.ToLower().Contains(peripheralFilter.ToLower());

        public static void RefreshCollection()
        {
            PeripheralsViewModel.Peripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Peripherals.Include(manufacturer => manufacturer.Manufacturer).Include(typePeripheral => typePeripheral.Types_peripherals))
                PeripheralsViewModel.Peripherals.Add(item);
        }

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
