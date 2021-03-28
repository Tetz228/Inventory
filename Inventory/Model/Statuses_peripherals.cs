namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Statuses_peripherals : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Statuses_peripherals()
        {
            this.Inventory_numbers_peripherals = new HashSet<Inventory_numbers_peripherals>();
        }

        public int Id_status_peripheral { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_peripherals> Inventory_numbers_peripherals { get; set; }

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

        public bool IsValidationProperties() => ErrorCollection.Count == 0 || ErrorCollection.Any(item => item.Value == null);
        #endregion

        public static bool SearchFor(Statuses_peripherals statusPeripheral, string statusPeripheralFilter) => statusPeripheral.Name.ToLower().Contains(statusPeripheralFilter.ToLower());

        public static void RefreshCollection()
        {
            StatusesPeripheralsViewModel.StatusesPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Statuses_peripherals)
                StatusesPeripheralsViewModel.StatusesPeripherals.Add(item);
        }

        #region Откат изменений
        private Statuses_peripherals _selectStatusPeripheral;

        public void BeginEdit()
        {
            _selectStatusPeripheral = new Statuses_peripherals
            {
                Id_status_peripheral = this.Id_status_peripheral,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectStatusPeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectStatusPeripheral == null)
                return;

            Id_status_peripheral = _selectStatusPeripheral.Id_status_peripheral;
            Name = _selectStatusPeripheral.Name;
        }
        #endregion
    }
}
