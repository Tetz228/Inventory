namespace Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;
    using Services;

    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    public class InventoryGraphicCardEditViewModel : BindableBase
    {
        public InventoryGraphicCardEditViewModel(Inventory_numbers_graphics_cards inventoryGraphicCard)
        {
            using var db = new InventoryEntities();

            GraphicsCards = new ObservableCollection<Graphics_cards>(db.Graphics_cards.AsNoTracking()
                    .Include(manufacturer => manufacturer.Manufacturer)
                    .Include(unit => unit.Unit))
                    .Sort(manufact => manufact.Manufacturer.Name);
            InventoryGraphicCard = inventoryGraphicCard;
            InventoryGraphicCard.BeginEdit();
        }

        public Inventory_numbers_graphics_cards InventoryGraphicCard { get; }

        public ObservableCollection<Graphics_cards> GraphicsCards { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryGraphicCard.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryGraphicCard.EndEdit();
            Services.Edit(InventoryGraphicCard.Id_inventory_number_graphics_card, InventoryGraphicCard);
            InventoryGraphicCardViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryGraphicCard.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryGraphicCard.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
