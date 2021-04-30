namespace Inventory.ViewModels.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
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
            RefreshCollection();
            GraphicsCardsCollection = CollectionViewSource.GetDefaultView(GraphicsCards);
        }

        #region Свойства
        private ICollectionView GraphicsCardsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Graphics_cards SelectGraphicCard { get; set; }

        public static ObservableCollection<Graphics_cards> GraphicsCards { get; set; } = new();

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
                        return graphicCard.Search(GraphicsCardsFilter);

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
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            GraphicsCards.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            GraphicsCards.Sort(graphicCard => graphicCard.Name, SortDirection);
                            break;
                        }
                    case "Объём":
                        {
                            GraphicsCards.Sort(graphicCard => graphicCard.Memory_size + " " + graphicCard.Unit.Short_name, SortDirection);
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

        public ICommand DeleteGraphicCardCommand => new DelegateCommand<Graphics_cards>(selectGraphicCard =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить видеокарту:\nпроизводитель - {selectGraphicCard.Manufacturer.Name};\nнаименование - {selectGraphicCard.Name};\nобъём - {selectGraphicCard.Memory_size} {selectGraphicCard.Unit.Short_name}?", "Удаление видеокарты", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Graphics_cards>(selectGraphicCard.Id_graphics_card);
            RefreshCollection();
        }, selectGraphicCard => selectGraphicCard != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            GraphicsCards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Graphics_cards.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit))
                GraphicsCards.Add(item);

            GraphicsCards.Sort(manufacturer => manufacturer.Manufacturer.Name);
        }
    }
}
