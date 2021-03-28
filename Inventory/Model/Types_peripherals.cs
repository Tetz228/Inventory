namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Types_peripherals : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Types_peripherals()
        {
            this.Peripherals = new HashSet<Peripheral>();
        }

        public int Id_type_peripheral { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Peripheral> Peripherals { get; set; }

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

        public static bool SearchFor(Types_peripherals typePeripheral, string typePeripheralFilter) => typePeripheral.Name.ToLower().Contains(typePeripheralFilter.ToLower());

        public static void RefreshCollection()
        {
            TypesPeripheralsViewModel.TypesPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_peripherals)
                TypesPeripheralsViewModel.TypesPeripherals.Add(item);
        }

        #region Откат изменений
        private Types_peripherals _selectTypePeripheral;

        public void BeginEdit()
        {
            _selectTypePeripheral = new Types_peripherals
            {
                Id_type_peripheral = this.Id_type_peripheral,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectTypePeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectTypePeripheral == null)
                return;

            Id_type_peripheral = _selectTypePeripheral.Id_type_peripheral;
            Name = _selectTypePeripheral.Name;
        }
        #endregion
    }
}
