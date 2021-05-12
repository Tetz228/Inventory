namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Inventory_numbers_ram : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_ram() => Ram_in_computers = new HashSet<Ram_in_computers>();

        public int Id_inventory_number_ram { get; set; }
        public int Fk_ram { get; set; }
        public int Inventory_number { get; set; }

        public virtual Ram Ram { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ram_in_computers> Ram_in_computers { get; set; }

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
                        else if (Services.CheckForUniqueness<Inventory_numbers_ram>(nameof(Inventory_number), Inventory_number, _selectInventoryRam?.Inventory_number))
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
        private Inventory_numbers_ram _selectInventoryRam;

        public void BeginEdit()
        {
            _selectInventoryRam = new Inventory_numbers_ram
            {
                Id_inventory_number_ram = Id_inventory_number_ram,
                Inventory_number = Inventory_number,
                Fk_ram = Fk_ram
            };
        }

        public void EndEdit() => _selectInventoryRam = null;

        public void CancelEdit()
        {
            if (_selectInventoryRam == null)
                return;

            Id_inventory_number_ram = _selectInventoryRam.Id_inventory_number_ram;
            Inventory_number = _selectInventoryRam.Inventory_number;
            Fk_ram = _selectInventoryRam.Fk_ram;
        }
        #endregion
    }
}
