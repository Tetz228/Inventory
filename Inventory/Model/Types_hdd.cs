namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Other;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Types_hdd : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Types_hdd()
        {
            this.Hdds = new HashSet<Hdd>();
        }

        public int Id_type_hdd { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hdd> Hdds { get; set; }

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

        public static bool SearchFor(Types_hdd typeHdd, string typeHddFilter) => typeHdd.Name.ToLower().Contains(typeHddFilter.ToLower());

        public static void RefreshCollection()
        {
            TypesHddsViewModel.TypesHdds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_hdd)
                TypesHddsViewModel.TypesHdds.Add(item);
        }

        #region Откат изменений
        private Types_hdd _selectTypeHdd;

        public void BeginEdit()
        {
            _selectTypeHdd = new Types_hdd
            {
                Id_type_hdd = this.Id_type_hdd,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectTypeHdd = null;
        }

        public void CancelEdit()
        {
            if (_selectTypeHdd == null)
                return;

            Id_type_hdd = _selectTypeHdd.Id_type_hdd;
            Name = _selectTypeHdd.Name;
        }
        #endregion
    }
}