namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using DevExpress.Mvvm;

    public partial class Inventory_numbers_peripherals : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_peripherals()
        {
            List_dispensed_peripherals = new HashSet<List_dispensed_peripherals>();
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
                    case "Inventory_number":
                        if (Inventory_number <= 0)
                            result = "Поле должно содержать число больше нуля";
                        else
                        {
                            if (_selectInventoryNumberPeripheral == null)
                            {
                                if (IsUniqueInventoryNumber())
                                    result = "Номер должен быть уникальным";
                            }
                            else
                            {
                                if (_selectInventoryNumberPeripheral.Inventory_number != Inventory_number)
                                    if (IsUniqueInventoryNumber())
                                        result = "Номер должен быть уникальным";
                            }
                        }
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public bool IsUniqueInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isUniqueNumber = db.Inventory_numbers_peripherals.FirstOrDefault(number => number.Inventory_number == Inventory_number);

            return isUniqueNumber != null;
        }

        public static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_peripherals.FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_peripherals.Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        public bool IsValidationProperties() => ErrorCollection.Count == 0
                                                || ErrorCollection.Any(item => item.Value == null)
                                                && Fk_peripheral != 0 && Fk_status_peripheral != 0;
        #endregion

        #region Откат изменений
        private Inventory_numbers_peripherals _selectInventoryNumberPeripheral;

        public void BeginEdit()
        {
            _selectInventoryNumberPeripheral = new Inventory_numbers_peripherals()
            {
                Id_inventory_number_peripheral = Id_inventory_number_peripheral,
                Inventory_number = Inventory_number,
                Fk_status_peripheral = Fk_status_peripheral,
                Fk_peripheral = Fk_peripheral
            };
        }

        public void EndEdit()
        {
            _selectInventoryNumberPeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectInventoryNumberPeripheral == null)
                return;

            Id_inventory_number_peripheral = _selectInventoryNumberPeripheral.Id_inventory_number_peripheral;
            Inventory_number = _selectInventoryNumberPeripheral.Inventory_number;
            Fk_status_peripheral = _selectInventoryNumberPeripheral.Fk_status_peripheral;
            Fk_peripheral = _selectInventoryNumberPeripheral.Fk_peripheral;
        }
        #endregion
    }
}