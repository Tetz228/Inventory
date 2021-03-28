namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class GraphicCardEditViewModel : BindableBase, IEditableObject
    {
        public GraphicCardEditViewModel(Graphics_cards graphicCard)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Units = new ObservableCollection<Unit>(db.Units);

            GraphicCard = graphicCard;
            BeginEdit();
        }

        public Graphics_cards GraphicCard { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(GraphicCard.Id_graphics_card, GraphicCard);
            GraphicsCardsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => GraphicCard.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Graphics_cards _selectGraphicCard;

        public void BeginEdit()
        {
            _selectGraphicCard = new Graphics_cards()
            {
                Id_graphics_card = GraphicCard.Id_graphics_card,
                Memory_size = GraphicCard.Memory_size,
                Name = GraphicCard.Name,
                Fk_unit = GraphicCard.Fk_unit,
                Fk_manufacturer = GraphicCard.Fk_manufacturer,
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

            GraphicCard.Id_graphics_card = _selectGraphicCard.Id_graphics_card;
            GraphicCard.Memory_size = _selectGraphicCard.Memory_size;
            GraphicCard.Name = _selectGraphicCard.Name;
            GraphicCard.Fk_unit = _selectGraphicCard.Fk_unit;
            GraphicCard.Fk_manufacturer = _selectGraphicCard.Fk_manufacturer;
        }
        #endregion
    }
}
