namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    public partial class Types_hdd : BindableBase, IEditableObject, IDataErrorInfo
    {
        public Types_hdd()
        {
            this.Hdds = new HashSet<Hdd>();
        }

        public int Id_type_hdd { get; set; }
        public string Name { get; set; }
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

        #region Метод поиска
        public static bool Search(Types_hdd typeHdd, string typeHddFilter) => typeHdd.Name.ToLower().Contains(typeHddFilter.ToLower());
        #endregion

        #region Методы обработки информации
        public static void AddTypeHdd(string name)
        {
            using var db = new InventoryEntities();

            var typeHdd = new Types_hdd()
            {
                Name = name
            };

            db.Types_hdd.Add(typeHdd);
            db.SaveChanges();

            TypesHddViewModel.TypesHdds.Add(typeHdd);
        }

        public static void EditTypeHdd(Types_hdd typeHdd)
        {
            using var db = new InventoryEntities();
            var findTypeHdd = db.Types_hdd.FirstOrDefault(type => type.Id_type_hdd == typeHdd.Id_type_hdd);

            if (findTypeHdd == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении типа жесткого диска",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            findTypeHdd.Name = typeHdd.Name;
            db.SaveChanges();
        }

        public static void DeleteTypeHdd(Types_hdd selectTypeHdd)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectTypeHdd.Name}?",
                "Удаление типа жесткого диска", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();
            var findTypeHdd = db.Types_hdd.FirstOrDefault(type => type.Id_type_hdd == selectTypeHdd.Id_type_hdd);

            if (findTypeHdd == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении типа жесткого диска", MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Types_hdd.Remove(findTypeHdd);
                db.SaveChanges();

                TypesHddViewModel.TypesHdds.Remove(selectTypeHdd);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить тип жесткого диска, так как он связана с другими сущностями!",
                    "Ошибка при удалении типа жесткого диска", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            TypesHddViewModel.TypesHdds.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Types_hdd)
                TypesHddViewModel.TypesHdds.Add(item);
        }
        #endregion

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
