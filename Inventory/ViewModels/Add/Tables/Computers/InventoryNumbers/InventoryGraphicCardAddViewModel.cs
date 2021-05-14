namespace Inventory.ViewModels.Add.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;
    using Services;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class InventoryGraphicCardAddViewModel : BindableBase
    {
        public InventoryGraphicCardAddViewModel()
        {
            using var db = new InventoryEntities();

            GraphicsCards = new ObservableCollection<Graphics_cards>(db.Graphics_cards.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(unit => unit.Unit))
                .Sort(manufact => manufact.Manufacturer.Name);

            InventoryGraphicCard.Inventory_number = db.Inventory_numbers_graphics_cards.FirstOrDefault() == null ? InventoryGraphicCard.Inventory_number = 1 : InventoryGraphicCard.Inventory_number = db.Inventory_numbers_graphics_cards.Select(graphicsCards => graphicsCards.Inventory_number).Max() + 1;
        }

        public Inventory_numbers_graphics_cards InventoryGraphicCard { get; } = new();

        public ObservableCollection<Graphics_cards> GraphicsCards { get; }
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryGraphicCard);
            InventoryGraphicsCardsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryGraphicCard.ErrorCollection));
    }
}
