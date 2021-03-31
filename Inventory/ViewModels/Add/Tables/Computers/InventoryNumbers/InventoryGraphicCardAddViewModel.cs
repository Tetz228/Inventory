namespace Inventory.ViewModels.Add.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;
    using Services;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    public class InventoryGraphicCardAddViewModel : BindableBase
    {
        public InventoryGraphicCardAddViewModel()
        {
            using var db = new InventoryEntities();
            InventoryGraphicCard = new Inventory_numbers_graphics_cards();
            GraphicsCards = new ObservableCollection<Graphics_cards>(db.Graphics_cards.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit));
            InventoryGraphicCard.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_graphics_cards InventoryGraphicCard { get; }

        public ObservableCollection<Graphics_cards> GraphicsCards { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_graphics_cards.FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_graphics_cards.Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryGraphicCard);
            InventoryGraphicCardViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryGraphicCard.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
