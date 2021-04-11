namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using DevExpress.Mvvm;

    public partial class Power_supplies : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Power_supplies() => this.Inventory_numbers_power_supplies = new HashSet<Inventory_numbers_power_supplies>();

        public int Id_power_supplie { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public int Power { get; set; }
        public int Fk_unit { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_power_supplies> Inventory_numbers_power_supplies { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Unit Unit { get; set; }

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
                    case "Power":
                        if (Power <= 0)
                            result = "Число должно быть больше 0";
                        break;
                    case "Fk_manufacturer":
                        if (Fk_manufacturer == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "Fk_unit":
                        if (Fk_unit == 0)
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
    }
}
