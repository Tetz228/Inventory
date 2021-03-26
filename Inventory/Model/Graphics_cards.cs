namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    public partial class Graphics_cards : BindableBase, IEditableObject, IDataErrorInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Graphics_cards()
        {
            this.Inventory_numbers_graphics_cards = new HashSet<Inventory_numbers_graphics_cards>();
        }

        public int Id_graphics_card { get; set; }
        public int Fk_manufacturer { get; set; }
        public string Name { get; set; }
        public double Memory_size { get; set; }
        public int Fk_unit { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventory_numbers_graphics_cards> Inventory_numbers_graphics_cards { get; set; }

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
                    case "Memory_size":
                        if (Memory_size <= 0)
                            result = "Поле должно быть больше 0";
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
                                                && Fk_unit != 0;

        #endregion

        public static bool SearchFor(Graphics_cards graphicCard, string graphicCardFilter) => graphicCard.Name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                              || graphicCard.Unit.Full_name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                              || graphicCard.Memory_size.ToString(CultureInfo.InvariantCulture).ToLower().Contains(graphicCardFilter.ToLower())
                                                                              || graphicCard.Unit.Short_name.ToLower().Contains(graphicCardFilter.ToLower())
                                                                              || graphicCard.Manufacturer.Name.ToLower().Contains(graphicCardFilter.ToLower());

        #region Методы обработки информации
        public static void AddGraphicCard(Graphics_cards graphicCard)
        {
            using var db = new InventoryEntities();

            var newGraphicCard = new Graphics_cards()
            {
                Memory_size = graphicCard.Memory_size,
                Name = graphicCard.Name,
                Fk_unit = graphicCard.Fk_unit,
                Fk_manufacturer = graphicCard.Fk_manufacturer
            };

            db.Graphics_cards.Add(newGraphicCard);
            db.SaveChanges();

            newGraphicCard.Manufacturer = db.Manufacturers.FirstOrDefault(manufacturer => manufacturer.Id_manufacturer == newGraphicCard.Fk_manufacturer);
            newGraphicCard.Unit = db.Units.FirstOrDefault(unit => unit.Id_unit == newGraphicCard.Fk_unit);

            GraphicsCardsViewModel.GraphicsCards.Add(newGraphicCard);
        }

        public static void EditGraphicCard(Graphics_cards graphicCard)
        {
            using var db = new InventoryEntities();
            var foundGraphicCard = db.Graphics_cards.FirstOrDefault(card => card.Id_graphics_card == graphicCard.Id_graphics_card);

            if (foundGraphicCard == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при изменении видеокарты",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            foundGraphicCard.Memory_size = graphicCard.Memory_size;
            foundGraphicCard.Name = graphicCard.Name;
            foundGraphicCard.Fk_unit = graphicCard.Fk_unit;
            foundGraphicCard.Fk_manufacturer = graphicCard.Fk_manufacturer;

            db.SaveChanges();

            RefreshCollection();
        }

        public static void DeleteGraphicCard(Graphics_cards selectGraphicCard)
        {
            if (MessageBoxResult.Yes != MessageBox.Show($"Вы действительно хотите удалить - {selectGraphicCard.Name}?",
                "Удаление видеокарты", MessageBoxButton.YesNo, MessageBoxImage.Question))
                return;

            using var db = new InventoryEntities();

            var foundGraphicCard = db.Graphics_cards.FirstOrDefault(graphicCard => graphicCard.Id_graphics_card == selectGraphicCard.Id_graphics_card);

            if (foundGraphicCard == null)
            {
                MessageBox.Show("Объект не найден в базе данных!", "Ошибка при удалении видеокарты",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                RefreshCollection();
                return;
            }

            try
            {
                db.Graphics_cards.Remove(foundGraphicCard);
                db.SaveChanges();

                GraphicsCardsViewModel.GraphicsCards.Remove(selectGraphicCard);
            }
            catch (DbUpdateException)
            {
                MessageBox.Show("Невозможно удалить видеокарту, так как она связана с другими сущностями!",
                    "Ошибка при удалении видеокарты", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void RefreshCollection()
        {
            GraphicsCardsViewModel.GraphicsCards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Graphics_cards.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit))
                GraphicsCardsViewModel.GraphicsCards.Add(item);
        }
        #endregion

        #region Откат изменений
        private Graphics_cards _selectGraphicCard;

        public void BeginEdit()
        {
            _selectGraphicCard = new Graphics_cards()
            {
                Id_graphics_card = this.Id_graphics_card,
                Memory_size = this.Memory_size,
                Name = this.Name,
                Fk_unit = this.Fk_unit,
                Fk_manufacturer = this.Fk_manufacturer,
            };
        }

        public void EndEdit()
        {
            _selectGraphicCard = null;
        }

        public void CancelEdit()
        {
            if (_selectGraphicCard == null)
                return;

            Id_graphics_card = _selectGraphicCard.Id_graphics_card;
            Memory_size = _selectGraphicCard.Memory_size;
            Name = _selectGraphicCard.Name;
            Fk_unit = _selectGraphicCard.Fk_unit;
            Fk_manufacturer = _selectGraphicCard.Fk_manufacturer;
        }
        #endregion
    }
}