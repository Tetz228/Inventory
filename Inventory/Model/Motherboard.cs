namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Windows;

    public partial class Motherboard : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Motherboard()
        {
            this.Inventory_numbers_motherboards = new HashSet<Inventory_numbers_motherboards>();
        }

        public int Id_motherboard { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public int Fk_socket { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_motherboards> Inventory_numbers_motherboards { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Socket Socket { get; set; }


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

        public string Error
        {
            get => null;
        }

        public bool IsValidationProperties() => ErrorCollection.Count == 0
                                                || ErrorCollection.Any(item => item.Value == null)
                                                && Fk_manufacturer != 0
                                                && Fk_socket != 0;

        #endregion

        public static bool SearchFor(Motherboard motherboard, string motherboardFilter) => motherboard.Name.ToLower().Contains(motherboardFilter.ToLower())
                                                                              || motherboard.Socket.Name.ToLower().Contains(motherboardFilter.ToLower())
                                                                              || motherboard.Manufacturer.Name.ToLower().Contains(motherboardFilter.ToLower());

        #region Методы обработки информации
        public static void AddMotherboard(Motherboard motherboard)
        {
            using var db = new InventoryEntities();

            var newMotherboard = new Motherboard()
            {
                Fk_socket = motherboard.Fk_socket,
                Name = motherboard.Name,
                Fk_manufacturer = motherboard.Fk_manufacturer
            };

            db.Motherboards.Add(newMotherboard);
            db.SaveChanges();

            newMotherboard.Manufacturer = db.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Id_manufacturer == newMotherboard.Fk_manufacturer);
            newMotherboard.Socket = db.Sockets.FirstOrDefault(socket => socket.Id_socket == newMotherboard.Fk_socket);

            MotherboardsViewModel.Motherboards.Add(newMotherboard);
        }

        public static void EditMotherboard(Motherboard motherboard)
        {
            using var db = new InventoryEntities();
            var foundMotherboard = db.Motherboards.FirstOrDefault(mother => mother.Id_motherboard == motherboard.Id_motherboard);

            if (foundMotherboard == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении материнской платы",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            foundMotherboard.Name = motherboard.Name;
            foundMotherboard.Fk_socket = motherboard.Fk_socket;
            foundMotherboard.Fk_manufacturer = motherboard.Fk_manufacturer;

            db.SaveChanges();

            RefreshCollection();
        }

        public static void DeleteMotherboard(Motherboard selectMotherboard)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectMotherboard.Name}?",
                "Удаление материнской платы", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();

            var foundMotherboard = db.Motherboards.FirstOrDefault(motherboard => motherboard.Id_motherboard == selectMotherboard.Id_motherboard);

            if (foundMotherboard == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении материнской платы",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Motherboards.Remove(foundMotherboard);
                db.SaveChanges();

                MotherboardsViewModel.Motherboards.Remove(selectMotherboard);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить материнскую плату, так как она связана с другими сущностями!",
                    "Ошибка при удалении материнской платы", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            MotherboardsViewModel.Motherboards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Motherboards.Include(manufacturer => manufacturer.Manufacturer).Include(socket => socket.Socket))
                MotherboardsViewModel.Motherboards.Add(item);
        }
        #endregion

        #region Откат изменений
        private Motherboard _selectMotherboard;

        public void BeginEdit()
        {
            _selectMotherboard = new Motherboard()
            {
                Id_motherboard = this.Id_motherboard,
                Name = this.Name,
                Fk_socket = this.Fk_socket,
                Fk_manufacturer = this.Fk_manufacturer,
            };
        }

        public void EndEdit()
        {
            _selectMotherboard = null;
        }

        public void CancelEdit()
        {
            if (_selectMotherboard == null)
                return;

            Id_motherboard = _selectMotherboard.Id_motherboard;
            Name = _selectMotherboard.Name;
            Fk_socket = _selectMotherboard.Fk_socket;
            Fk_manufacturer = _selectMotherboard.Fk_manufacturer;
        }
        #endregion
    }
}
