namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Inventory_numbers_power_supplies : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_power_supplies() => Computers = new HashSet<Computer>();

        public int Id_inventory_number_power_supplie { get; set; }
        public int Fk_power_supplie { get; set; }
        public int Inventory_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Computer> Computers { get; set; }
        public virtual Power_supplies Power_supplies { get; set; }

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
                            result = "Число должно быть больше 0";
                        else if (Services.CheckForUniqueness<Inventory_numbers_power_supplies>(nameof(Inventory_number), Inventory_number, _selectInventoryPowerSupplies?.Inventory_number))
                            result = "Номер должен быть уникальным";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        #endregion

        #region Откат изменений
        private Inventory_numbers_power_supplies _selectInventoryPowerSupplies;

        public void BeginEdit()
        {
            _selectInventoryPowerSupplies = new Inventory_numbers_power_supplies
            {
                Id_inventory_number_power_supplie = Id_inventory_number_power_supplie,
                Inventory_number = Inventory_number,
                Fk_power_supplie = Fk_power_supplie
            };
        }

        public void EndEdit() => _selectInventoryPowerSupplies = null;

        public void CancelEdit()
        {
            if (_selectInventoryPowerSupplies == null)
                return;

            Id_inventory_number_power_supplie = _selectInventoryPowerSupplies.Id_inventory_number_power_supplie;
            Inventory_number = _selectInventoryPowerSupplies.Inventory_number;
            Fk_power_supplie = _selectInventoryPowerSupplies.Fk_power_supplie;
        }
        #endregion
    }
}
