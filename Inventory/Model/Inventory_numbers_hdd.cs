namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Inventory_numbers_hdd : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_hdd()
        {
            this.Hdd_in_computers = new HashSet<Hdd_in_computers>();
        }

        public string InventoryNumberString { get; set; }

        public int Id_inventory_number_hdd { get; set; }
        public int Fk_hdd { get; set; }
        public int Inventory_number { get; set; }

        public virtual Hdd Hdd { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hdd_in_computers> Hdd_in_computers { get; set; }

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
                            result = Services.ValidInventoryNumber(InventoryNumberString, _selectInventoryNumberHdd?.Inventory_number, this);
                        break;
                    case "Fk_hdd":
                        if (Fk_hdd == 0)
                            result = "Поле не должно быть пустым";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        public string IsUniqueInventoryNumber(int inventoryNumber)
        {
            using var db = new InventoryEntities();
            var isUniqueNumber = db.Inventory_numbers_hdd.FirstOrDefault(number => number.Inventory_number == inventoryNumber);

            return isUniqueNumber == null ? null : "Номер должен быть уникальным";
        }

        #endregion

        #region Откат изменений
        private Inventory_numbers_hdd _selectInventoryNumberHdd;

        public void BeginEdit()
        {
            _selectInventoryNumberHdd = new Inventory_numbers_hdd()
            {
                Id_inventory_number_hdd = Id_inventory_number_hdd,
                Inventory_number = Inventory_number,
                Fk_hdd = Fk_hdd
            };
        }

        public void EndEdit()
        {
            _selectInventoryNumberHdd = null;
        }

        public void CancelEdit()
        {
            if (_selectInventoryNumberHdd == null)
                return;

            Id_inventory_number_hdd = _selectInventoryNumberHdd.Id_inventory_number_hdd;
            Inventory_number = _selectInventoryNumberHdd.Inventory_number;
            Fk_hdd = _selectInventoryNumberHdd.Fk_hdd;
        }
        #endregion
    }
}
