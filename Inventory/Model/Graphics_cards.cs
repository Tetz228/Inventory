namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Graphics_cards : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Graphics_cards() => Inventory_numbers_graphics_cards = new HashSet<Inventory_numbers_graphics_cards>();

        public int Id_graphics_card { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public double Memory_size { get; set; }
        public int Fk_unit { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_graphics_cards> Inventory_numbers_graphics_cards { get; set; }

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
                    case "Memory_size":
                         if (Memory_size <= 0)
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