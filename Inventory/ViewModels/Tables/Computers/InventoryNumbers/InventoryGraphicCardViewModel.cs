namespace Inventory.ViewModels.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers.InventoryNumbers;
    using Inventory.View.Edit.Tables.Computers.InventoryNumbers;
    using Services;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers;

    public class InventoryGraphicCardViewModel : BindableBase
    {
        public InventoryGraphicCardViewModel()
        {
            using var db = new InventoryEntities();

            InventoryGraphicsCards = new ObservableCollection<Inventory_numbers_graphics_cards>(db.Inventory_numbers_graphics_cards.Include(manufacturer => manufacturer.Graphics_cards.Manufacturer).Include(unit => unit.Graphics_cards.Unit));
            InventoryGraphicsCards.Sort(manufacturer => manufacturer.Inventory_number, SortDirection = ListSortDirection.Ascending);
            InventoryGraphicsCardsCollection = CollectionViewSource.GetDefaultView(InventoryGraphicsCards);
        }

        #region Свойства
        private ICollectionView InventoryGraphicsCardsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Inventory_numbers_graphics_cards SelectInventoryGraphicCard { get; set; }

        public static ObservableCollection<Inventory_numbers_graphics_cards> InventoryGraphicsCards { get; set; }

        private string _inventoryGraphicsCardsFilter = string.Empty;

        public string InventoryGraphicsCardsFilter
        {
            get => _inventoryGraphicsCardsFilter;
            set
            {
                _inventoryGraphicsCardsFilter = value;
                InventoryGraphicsCardsCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_graphics_cards inventoryGraphicsCards)
                        return inventoryGraphicsCards.Search(InventoryGraphicsCardsFilter);

                    return false;
                };
                InventoryGraphicsCardsCollection.Refresh();
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
                    case "Инвентарный номер":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryGraphicsCards.Sort(numberHdd => numberHdd.Inventory_number, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryGraphicsCards.Sort(numberHdd => numberHdd.Inventory_number, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Производитель":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryGraphicsCards.Sort(manufacturer => manufacturer.Graphics_cards.Manufacturer.Name,
                                    SortDirection = ListSortDirection.Descending);
                            else
                                InventoryGraphicsCards.Sort(manufacturer => manufacturer.Graphics_cards.Manufacturer.Name,
                                    SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryGraphicsCards.Sort(graphicCard => graphicCard.Graphics_cards.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryGraphicsCards.Sort(graphicCard => graphicCard.Graphics_cards.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Объём":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryGraphicsCards.Sort(graphicCard => graphicCard.Graphics_cards.Memory_size, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryGraphicsCards.Sort(graphicCard => graphicCard.Graphics_cards.Memory_size, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Единица измерения":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryGraphicsCards.Sort(unit => unit.Graphics_cards.Unit.Full_name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryGraphicsCards.Sort(unit => unit.Graphics_cards.Unit.Full_name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryGraphicCard = null;
        #endregion

        #region Команды
        public ICommand AddInventoryGraphicCardCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryGraphicCardAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryGraphicCardCommand => new DelegateCommand<Inventory_numbers_graphics_cards>(inventoryGraphicsCards =>
        {
            var editWindow = new InventoryGraphicCardEditWindow();
            var editViewModel = new InventoryGraphicCardEditViewModel(inventoryGraphicsCards);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventoryGraphicsCards => inventoryGraphicsCards != null);

        public ICommand DeleteInventoryGraphicCardCommand => new DelegateCommand<Inventory_numbers_graphics_cards>(selectInventoryGraphicCard =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectInventoryGraphicCard.Graphics_cards.Manufacturer.Name} {selectInventoryGraphicCard.Graphics_cards.Name} {selectInventoryGraphicCard.Graphics_cards.Memory_size} {selectInventoryGraphicCard.Graphics_cards.Unit.Short_name}?", "Удаление инвентарной видеокарты", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Inventory_numbers_graphics_cards>(selectInventoryGraphicCard.Id_inventory_number_graphics_card);
            RefreshCollection();
        }, selectInventoryGraphicCard => selectInventoryGraphicCard != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryGraphicsCards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_graphics_cards.Include(manufacturer => manufacturer.Graphics_cards.Manufacturer).Include(unit => unit.Graphics_cards.Unit))
                InventoryGraphicsCards.Add(item);
        }
    }
}
