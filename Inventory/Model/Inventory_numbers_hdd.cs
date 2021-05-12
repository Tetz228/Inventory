namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Inventory_numbers_hdd : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_hdd() => Hdd_in_computers = new HashSet<Hdd_in_computers>();

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
                    case "Inventory_number":
                        if (Inventory_number <= 0)
                            result = "Число должно быть больше 0";
                        else if (Services.CheckForUniqueness<Inventory_numbers_hdd>(nameof(Inventory_number), Inventory_number, _selectInventoryHdd?.Inventory_number))
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
        private Inventory_numbers_hdd _selectInventoryHdd;

        public void BeginEdit()
        {
            _selectInventoryHdd = new Inventory_numbers_hdd
            {
                Id_inventory_number_hdd = Id_inventory_number_hdd,
                Inventory_number = Inventory_number,
                Fk_hdd = Fk_hdd
            };
        }

        public void EndEdit() => _selectInventoryHdd = null;

        public void CancelEdit()
        {
            if (_selectInventoryHdd == null)
                return;

            Id_inventory_number_hdd = _selectInventoryHdd.Id_inventory_number_hdd;
            Inventory_number = _selectInventoryHdd.Inventory_number;
            Fk_hdd = _selectInventoryHdd.Fk_hdd;
        }
        #endregion
    }
}
