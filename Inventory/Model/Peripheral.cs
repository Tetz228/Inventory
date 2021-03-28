namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using DevExpress.Mvvm;

    public partial class Peripheral : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Peripheral()
        {
            Inventory_numbers_peripherals = new HashSet<Inventory_numbers_peripherals>();
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
        #endregion
    }
}
