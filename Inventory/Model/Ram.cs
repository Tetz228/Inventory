namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Ram : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ram() => Inventory_numbers_ram = new HashSet<Inventory_numbers_ram>();

        public int Id_ram { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public int Fk_type_memory { get; set; }
        public double Memory_size { get; set; }
        public double Clock_frequency { get; set; }
        public int Fk_unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_ram> Inventory_numbers_ram { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Types_memory Types_memory { get; set; }
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
                    case "Clock_frequency":
                        if (Clock_frequency <= 0)
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
                    case "Fk_type_memory":
                        if (Fk_type_memory == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "Memory_size":
                        if (Memory_size <= 0)
                            result = "Число должно быть больше 0";
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
