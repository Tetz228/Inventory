namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Computers;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Operating_systems : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Operating_systems()
        {
            this.Operating_systems_in_computers = new HashSet<Operating_systems_in_computers>();
        }

        public int Id_operating_system { get; set; }
        public string Name { get; set; }
        public string System_version { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operating_systems_in_computers> Operating_systems_in_computers { get; set; }

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
                    case "System_version":
                        if (string.IsNullOrWhiteSpace(System_version))
                            result = "Поле не должно быть пустым";
                        else if (System_version.Length < 2)
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

        public static bool SearchFor(Operating_systems operatingSystem, string operatingSystemsFilter) => operatingSystem.Name.ToLower().Contains(operatingSystemsFilter.ToLower()) || operatingSystem.System_version.ToLower().Contains(operatingSystemsFilter.ToLower());

        public static void RefreshCollection()
        {
            OperatingSystemsViewModel.OperatingSystems.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Operating_systems)
                OperatingSystemsViewModel.OperatingSystems.Add(item);
        }

        #region Откат изменений
        private Operating_systems _selectOperatingSystem;

        public void BeginEdit()
        {
            _selectOperatingSystem = new Operating_systems
            {
                Id_operating_system = this.Id_operating_system,
                Name = this.Name,
                System_version = this.System_version
            };
        }

        public void EndEdit()
        {
            _selectOperatingSystem = null;
        }

        public void CancelEdit()
        {
            if (_selectOperatingSystem == null)
                return;

            Id_operating_system = _selectOperatingSystem.Id_operating_system;
            Name = _selectOperatingSystem.Name;
            System_version = _selectOperatingSystem.System_version;
        }
        #endregion
    }
}
