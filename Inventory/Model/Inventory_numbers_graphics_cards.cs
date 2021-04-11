namespace Inventory.Model
{
    using DevExpress.Mvvm;
    using Services;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Inventory_numbers_graphics_cards : BindableBase, IDataErrorInfo, IEditableObject
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inventory_numbers_graphics_cards() => Computers = new HashSet<Computer>();

        public int Id_inventory_number_graphics_card { get; set; }
        public int Fk_graphics_card { get; set; }
        public int Inventory_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Computer> Computers { get; set; }
        public virtual Graphics_cards Graphics_cards { get; set; }

        #region Валидация
        public Dictionary<string, string> ErrorCollection { get; private set; } = new();

        public string this[string name]
        {
            get
            {
                string result = null;

                switch (name)
                {
                    case "Inventory_number":
                        result = Inventory_number <= 0 ? "Число должно быть больше 0" : Services.ValidInventoryNumber<Inventory_numbers_graphics_cards>(Inventory_number, _selectInventoryGraphicCard?.Inventory_number);
                        break;
                    case "Fk_graphics_card":
                        if (Fk_graphics_card == 0)
                            result = "Поле не должно быть пустым";
                        break;
                }

                ErrorCollection[name] = result;

                RaisePropertyChanged(nameof(ErrorCollection));

                return result;
            }
        }

        public string Error { get => null; }

        #endregion

        #region Откат изменений
        private Inventory_numbers_graphics_cards _selectInventoryGraphicCard;

        public void BeginEdit()
        {
            _selectInventoryGraphicCard = new Inventory_numbers_graphics_cards
            {
                Id_inventory_number_graphics_card = Id_inventory_number_graphics_card,
                Inventory_number = Inventory_number,
                Fk_graphics_card = Fk_graphics_card
            };
        }

        public void EndEdit() => _selectInventoryGraphicCard = null;

        public void CancelEdit()
        {
            if (_selectInventoryGraphicCard == null)
                return;

            Id_inventory_number_graphics_card = _selectInventoryGraphicCard.Id_inventory_number_graphics_card;
            Inventory_number = _selectInventoryGraphicCard.Inventory_number;
            Fk_graphics_card = _selectInventoryGraphicCard.Fk_graphics_card;
        }
        #endregion
    }
}
