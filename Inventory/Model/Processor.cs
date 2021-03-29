namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Processor : BindableBase, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Processor()
        {
            this.Inventory_numbers_processors = new HashSet<Inventory_numbers_processors>();
        }

        public string BaseFrequencyString { get; set; }

        public string AmountCoresString { get; set; }

        public int Id_processor { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public int Fk_socket { get; set; }
        public int Amount_cores { get; set; }
        public double Base_frequency { get; set; }
        public int Fk_unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_processors> Inventory_numbers_processors { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Socket Socket { get; set; }
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
                    case "BaseFrequencyString":
                        if (string.IsNullOrWhiteSpace(BaseFrequencyString))
                            result = "Поле не должно быть пустым";
                        else if (double.TryParse(BaseFrequencyString, out double _) == false)
                            result = "Некорректное поле";
                        else if (double.Parse(BaseFrequencyString) <= 0.0)
                            result = "Число должно быть больше 0";
                        else
                            Base_frequency = double.Parse(BaseFrequencyString);
                        break;
                    case "Fk_manufacturer":
                        if (Fk_manufacturer == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "Fk_unit":
                        if (Fk_unit == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "Fk_socket":
                        if (Fk_socket == 0)
                            result = "Поле не должно быть пустым";
                        break;
                    case "AmountCoresString":
                        if (string.IsNullOrWhiteSpace(AmountCoresString))
                            result = "Поле не должно быть пустым";
                        else if (int.TryParse(AmountCoresString, out int _) == false)
                            result = "Некорректное поле";
                        else if (int.Parse(AmountCoresString) <= 0)
                            result = "Число должно быть больше 0";
                        else
                            Amount_cores = int.Parse(AmountCoresString);
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
