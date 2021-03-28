namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Other;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Types_memory : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Types_memory()
        {
            this.Rams = new HashSet<Ram>();
        }

        public int Id_type_memory { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ram> Rams { get; set; }

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

        public static bool SearchFor(Types_memory typeMemory, string typeMemoryFilter) => typeMemory.Name.ToLower().Contains(typeMemoryFilter.ToLower());

        public static void RefreshCollection()
        {
            TypesMemoryViewModel.TypesMemory.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_memory)
                TypesMemoryViewModel.TypesMemory.Add(item);
        }

        #region Откат изменений
        private Types_memory _selectTypeMemory;

        public void BeginEdit()
        {
            _selectTypeMemory = new Types_memory
            {
                Id_type_memory = this.Id_type_memory,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectTypeMemory = null;
        }

        public void CancelEdit()
        {
            if (_selectTypeMemory == null)
                return;

            Id_type_memory = _selectTypeMemory.Id_type_memory;
            Name = _selectTypeMemory.Name;
        }
        #endregion
    }
}