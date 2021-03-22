//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;


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

        #region Метод поиска
        public static bool Search(Types_ssd typeSsd, string typeSsdFilter) => typeSsd.Name.ToLower().Contains(typeSsdFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddTypeSsd(string name)
        {
            using var db = new InventoryEntities();

            var typeSsd = new Types_ssd()
            {
                Name = name
            };

            db.Types_ssd.Add(typeSsd);
            db.SaveChanges();

            TypesSddsViewModel.TypesSsds.Add(typeSsd);
        }

        public static void EditTypeSsd(Types_ssd typeSsd)
        {
            using var db = new InventoryEntities();
            var findTypeSsd = db.Types_ssd.FirstOrDefault(type => type.Id_type_ssd == typeSsd.Id_type_ssd);

            if (findTypeSsd == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении типа SSD",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findTypeSsd.Name = typeSsd.Name;
            db.SaveChanges();
        }

        public static void DeleteTypeSsd(Types_ssd selectTypeSsd)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectTypeSsd.Name}?",
                "Удаление типа SSD", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findTypeSsd = db.Types_ssd.FirstOrDefault(type => type.Id_type_ssd == selectTypeSsd.Id_type_ssd);

            if (findTypeSsd == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении типа SSD", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Types_ssd.Remove(findTypeSsd);
                db.SaveChanges();

                TypesSddsViewModel.TypesSsds.Remove(selectTypeSsd);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить тип SSD, так как он связана с другими сущностями!",
                    "Ошибка при удалении типа SSD", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            TypesSddsViewModel.TypesSsds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_ssd)
                TypesSddsViewModel.TypesSsds.Add(item);
        }
        #endregion

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
