namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Inventory_numbers_peripherals : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_peripherals()
        {
            List_dispensed_peripherals = new HashSet<List_dispensed_peripherals>();
        }

        public string InventoryNumberString { get; set; }

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
                    case "InventoryNumberString":
                        if (string.IsNullOrWhiteSpace(InventoryNumberString))
                            result = "Поле не должно быть пустым";
                        else if (int.TryParse(InventoryNumberString, out int _) == false)
                            result = "Некорректное поле";
                        else if (int.Parse(InventoryNumberString) <= 0)
                            result = "Число должно быть больше 0";
                        else
                            result = ValidInventoryNumber();
                        break;
                    case "Fk_peripheral":
                        if (Fk_peripheral == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "Fk_status_peripheral":
                        if (Fk_status_peripheral == 0)
                            result = "Поле не должно быть пустым";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string ValidInventoryNumber()
        {
            string result = string.Empty;

            Inventory_number = int.Parse(InventoryNumberString);

            if (_selectInventoryNumberPeripheral == null)
            {
                result = IsUniqueInventoryNumber();
            }
            else
            {
                if (_selectInventoryNumberPeripheral.Inventory_number != Inventory_number)
                    result = IsUniqueInventoryNumber();
            }

            return result;
        }

        public string Error { get => null; }

        private string IsUniqueInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isUniqueNumber = db.Inventory_numbers_peripherals.FirstOrDefault(number => number.Inventory_number == Inventory_number);

            return isUniqueNumber == null ? null : "Номер должен быть уникальным";
        }

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