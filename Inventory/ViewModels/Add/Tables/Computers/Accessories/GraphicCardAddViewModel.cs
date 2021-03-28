namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class GraphicCardAddViewModel : BindableBase
    {
        public GraphicCardAddViewModel()
        {
            using var db = new InventoryEntities();

            GraphicCard = new Graphics_cards();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Units = new ObservableCollection<Unit>(db.Units);
        }

        public Graphics_cards GraphicCard { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(GraphicCard);
            Graphics_cards.RefreshCollection();
            addWindow.Close();
        }, _ => GraphicCard.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
