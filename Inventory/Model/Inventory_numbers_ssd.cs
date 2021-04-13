namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Services;
    using DevExpress.Mvvm;

    public partial class Inventory_numbers_ssd : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_ssd() => this.Ssd_in_computers = new HashSet<Ssd_in_computers>();

        public int Id_inventory_number_ssd { get; set; }
        public int Fk_ssd { get; set; }
        public int Inventory_number { get; set; }
    
        public virtual Ssd Ssd { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ssd_in_computers> Ssd_in_computers { get; set; }

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
                        else if (Services.CheckForUniqueness<Inventory_numbers_ssd>(nameof(Inventory_number), Inventory_number, _selectInventorySsd?.Inventory_number))
                            result = "Номер должен быть уникальным";
                        break;
                    case "Fk_ssd":
                        if (Fk_ssd == 0)
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
        private Inventory_numbers_ssd _selectInventorySsd;

        public void BeginEdit()
        {
            _selectInventorySsd = new Inventory_numbers_ssd
            {
                Id_inventory_number_ssd = Id_inventory_number_ssd,
                Inventory_number = Inventory_number,
                Fk_ssd = Fk_ssd
            };
        }

        public void EndEdit() => _selectInventorySsd = null;

        public void CancelEdit()
        {
            if (_selectInventorySsd == null)
                return;

            Id_inventory_number_ssd = _selectInventorySsd.Id_inventory_number_ssd;
            Inventory_number = _selectInventorySsd.Inventory_number;
            Fk_ssd = _selectInventorySsd.Fk_ssd;
        }
        #endregion
    }
}
