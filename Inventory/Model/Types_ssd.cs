namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Other;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class Types_ssd : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Types_ssd()
        {
            this.Ssds = new HashSet<Ssd>();
        }

        public int Id_type_ssd { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ssd> Ssds { get; set; }

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

        public static bool SearchFor(Types_ssd typeSsd, string typeSsdFilter) => typeSsd.Name.ToLower().Contains(typeSsdFilter.ToLower());

        public static void RefreshCollection()
        {
            TypesSddsViewModel.TypesSsds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_ssd)
                TypesSddsViewModel.TypesSsds.Add(item);
        }

        #region Откат изменений
        private Types_ssd _selectTypeSsd;

        public void BeginEdit()
        {
            _selectTypeSsd = new Types_ssd
            {
                Id_type_ssd = this.Id_type_ssd,
                Name = this.Name
            };
        }

        public void EndEdit()
        {
            _selectTypeSsd = null;
        }

        public void CancelEdit()
        {
            if (_selectTypeSsd == null)
                return;

            Id_type_ssd = _selectTypeSsd.Id_type_ssd;
            Name = _selectTypeSsd.Name;
        }
        #endregion
    }
}
