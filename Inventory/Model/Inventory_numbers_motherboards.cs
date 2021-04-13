namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Inventory_numbers_motherboards : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_motherboards() => Computers = new HashSet<Computer>();

        public int Id_inventory_number_motherboard { get; set; }
        public int Fk_motherboard { get; set; }
        public int Inventory_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Computer> Computers { get; set; }
        public virtual Motherboard Motherboard { get; set; }

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
                        else if (Services.CheckForUniqueness<Inventory_numbers_motherboards>(nameof(Inventory_number), Inventory_number, _selectInventoryMotherboard?.Inventory_number))
                            result = "Номер должен быть уникальным";
                        break;
                    case "Fk_motherboard":
                        if (Fk_motherboard == 0)
                            result = "Поле не должно быть пустым";
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
        private Inventory_numbers_motherboards _selectInventoryMotherboard;

        public void BeginEdit()
        {
            _selectInventoryMotherboard = new Inventory_numbers_motherboards
            {
                Id_inventory_number_motherboard = Id_inventory_number_motherboard,
                Inventory_number = Inventory_number,
                Fk_motherboard = Fk_motherboard
            };
        }

        public void EndEdit() => _selectInventoryMotherboard = null;

        public void CancelEdit()
        {
            if (_selectInventoryMotherboard == null)
                return;

            Id_inventory_number_motherboard = _selectInventoryMotherboard.Id_inventory_number_motherboard;
            Inventory_number = _selectInventoryMotherboard.Inventory_number;
            Fk_motherboard = _selectInventoryMotherboard.Fk_motherboard;
        }
        #endregion
    }
}
