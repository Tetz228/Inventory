namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using DevExpress.Mvvm;

    public partial class Ssd : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ssd()
        {
            this.Inventory_numbers_ssd = new HashSet<Inventory_numbers_ssd>();
        }

        public string MemorySizeString { get; set; }

        public int Id_ssd { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public int Fk_type_ssd { get; set; }
        public double Memory_size { get; set; }
        public int Fk_unit { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_ssd> Inventory_numbers_ssd { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Types_ssd Types_ssd { get; set; }
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
                    case "MemorySizeString":
                        if (string.IsNullOrWhiteSpace(MemorySizeString))
                            result = "Поле не должно быть пустым";
                        else if (double.TryParse(MemorySizeString, out double _) == false)
                            result = "Некорректное поле";
                        else if (double.Parse(MemorySizeString) <= 0)
                            result = "Число должно быть больше 0";
                        else
                            Memory_size = double.Parse(MemorySizeString);
                        break;
                    case "Fk_manufacturer":
                        if (Fk_manufacturer == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "Fk_unit":
                        if (Fk_unit == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "Fk_type_ssd":
                        if (Fk_type_ssd == 0)
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
