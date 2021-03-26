namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers.Accessories;
    using Inventory.View.Edit.Tables.Computers.Accessories;
    using Inventory.ViewModels.Edit.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class GraphicsCardsViewModel : BindableBase
    {
        public GraphicsCardsViewModel()
        {
            using var db = new InventoryEntities();

            GraphicsCards = new ObservableCollection<Graphics_cards>(db.Graphics_cards.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit));
            GraphicsCards.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            GraphicsCardsCollection = CollectionViewSource.GetDefaultView(GraphicsCards);
        }

        #region Свойства
        private ICollectionView GraphicsCardsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Graphics_cards SelectGraphicCard { get; set; }

        public static ObservableCollection<Graphics_cards> GraphicsCards { get; set; }

        private string _graphicsCardsFilter = string.Empty;

        public string GraphicsCardsFilter
        {
            get => _graphicsCardsFilter;
            set
            {
                _graphicsCardsFilter = value;
                GraphicsCardsCollection.Filter = obj =>
                {
                    if (obj is Graphics_cards graphicCard)
                        return Graphics_cards.SearchFor(graphicCard, GraphicsCardsFilter);

                    return false;
                };
                GraphicsCardsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                GraphicsCards.Sort(manufacturer => manufacturer.Manufacturer.Name,
                                    SortDirection = ListSortDirection.Descending);
                            else
                                GraphicsCards.Sort(manufacturer => manufacturer.Manufacturer.Name,
                                    SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                GraphicsCards.Sort(graphicCard => graphicCard.Name, SortDirection = ListSortDirection.Descending);
                            else
                                GraphicsCards.Sort(graphicCard => graphicCard.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Объём":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                GraphicsCards.Sort(graphicCard => graphicCard.Memory_size, SortDirection = ListSortDirection.Descending);
                            else
                                GraphicsCards.Sort(graphicCard => graphicCard.Memory_size, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Единица измерения":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                GraphicsCards.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Descending);
                            else
                                GraphicsCards.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectGraphicCard = null;
        #endregion

        #region Команды
        public ICommand AddGraphicCardCommand => new DelegateCommand(() =>
        {
            var addHddWindow = new GraphicCardAddWindow();
            addHddWindow.ShowDialog();
        });

        public ICommand EditGraphicCardCommand => new DelegateCommand<Graphics_cards>(graphicCard =>
        {
            var editWindow = new GraphicCardEditWindow();
            var editViewModel = new GraphicCardEditViewModel(graphicCard);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, graphicCard => graphicCard != null);

        public ICommand DeleteGraphicCardCommand => new DelegateCommand<Graphics_cards>(Graphics_cards.DeleteGraphicCard, selectGraphicCard => selectGraphicCard != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Graphics_cards.RefreshCollection);
        #endregion
    }
}
