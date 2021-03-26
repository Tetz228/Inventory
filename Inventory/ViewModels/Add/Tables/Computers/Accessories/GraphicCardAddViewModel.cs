using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class GraphicCardAddViewModel:BindableBase
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
            Graphics_cards.AddGraphicCard(GraphicCard);
            addWindow.Close();
        }, _ => GraphicCard.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
