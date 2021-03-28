namespace Inventory.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    using DevExpress.Mvvm;

    using Inventory.ViewModels.Tables.Computers;
    using Inventory.ViewModels.Tables.Computers.Computers;
    using Inventory.ViewModels.Tables.Peripherals;

    public partial class Statuses_computers : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Statuses_computers()
        {
            this.Computers = new HashSet<Computer>();
        }
    
        public int Id_status_computer { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Computer> Computers { get; set; }

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

        public static bool SearchFor(Statuses_computers statusComputer, string statusComputerFilter) => statusComputer.Name.ToLower().Contains(statusComputerFilter.ToLower());

        public static void RefreshCollection()
        {
            StatusesComputersViewModel.StatusesComputers.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Statuses_computers)
                StatusesComputersViewModel.StatusesComputers.Add(item);
        }

        #region Откат изменений
        private Statuses_computers _selectStatusComputer;

        public void BeginEdit()
        {
            _selectStatusComputer = new Statuses_computers
            {
                Id_status_computer = this.Id_status_computer,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectStatusComputer = null;
        }

        public void CancelEdit()
        {
            if (_selectStatusComputer == null)
                return;

            Id_status_computer = _selectStatusComputer.Id_status_computer;
            Name = _selectStatusComputer.Name;
        }
        #endregion
    }
}
