namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Inventory_numbers_processors : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_processors() => Processors_in_computers = new HashSet<Processors_in_computers>();

        public int Id_inventory_number_processor { get; set; }
        public int Fk_processor { get; set; }
        public int Inventory_number { get; set; }

        public virtual Processor Processor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Processors_in_computers> Processors_in_computers { get; set; }

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
                        else if (Services.CheckForUniqueness<Inventory_numbers_processors>(nameof(Inventory_number), Inventory_number, _selectInventoryProcessor?.Inventory_number))
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
        private Inventory_numbers_processors _selectInventoryProcessor;

        public void BeginEdit()
        {
            _selectInventoryProcessor = new Inventory_numbers_processors
            {
                Id_inventory_number_processor = Id_inventory_number_processor,
                Inventory_number = Inventory_number,
                Fk_processor = Fk_processor
            };
        }

        public void EndEdit() => _selectInventoryProcessor = null;

        public void CancelEdit()
        {
            if (_selectInventoryProcessor == null)
                return;

            Id_inventory_number_processor = _selectInventoryProcessor.Id_inventory_number_processor;
            Inventory_number = _selectInventoryProcessor.Inventory_number;
            Fk_processor = _selectInventoryProcessor.Fk_processor;
        }
        #endregion
    }
}
