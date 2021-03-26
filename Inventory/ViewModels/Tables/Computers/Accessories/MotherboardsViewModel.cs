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

    public class MotherboardsViewModel : BindableBase
    {
        public MotherboardsViewModel()
        {
            using var db = new InventoryEntities();

            Motherboards = new ObservableCollection<Motherboard>(db.Motherboards.Include(manufacturer => manufacturer.Manufacturer).Include(socket => socket.Socket));
            Motherboards.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            MotherboardsCollection = CollectionViewSource.GetDefaultView(Motherboards);
        }

        #region Свойства
        private ICollectionView MotherboardsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Motherboard SelectMotherboard { get; set; }

        public static ObservableCollection<Motherboard> Motherboards { get; set; }

        private string _motherboardsFilter = string.Empty;

        public string MotherboardsFilter
        {
            get => _motherboardsFilter;
            set
            {
                _motherboardsFilter = value;
                MotherboardsCollection.Filter = obj =>
                {
                    if (obj is Motherboard motherboard)
                        return Motherboard.SearchFor(motherboard, MotherboardsFilter);

                    return false;
                };
                MotherboardsCollection.Refresh();
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
                                Motherboards.Sort(manufacturer => manufacturer.Manufacturer.Name,
                                    SortDirection = ListSortDirection.Descending);
                            else
                                Motherboards.Sort(manufacturer => manufacturer.Manufacturer.Name,
                                    SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Motherboards.Sort(graphicCard => graphicCard.Name, SortDirection = ListSortDirection.Descending);
                            else
                                Motherboards.Sort(graphicCard => graphicCard.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Сокет":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                Motherboards.Sort(socket => socket.Socket.Name, SortDirection = ListSortDirection.Descending);
                            else
                                Motherboards.Sort(socket => socket.Socket.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectMotherboard = null;
        #endregion

        #region Команды
        public ICommand AddMotherboardCommand => new DelegateCommand(() =>
        {
            var addHddWindow = new MotherboardAddWindow();
            addHddWindow.ShowDialog();
        });

        public ICommand EditMotherboardCommand => new DelegateCommand<Motherboard>(motherboard =>
        {
            var editWindow = new MotherboardEditWindow();
            var editViewModel = new MotherboardEditViewModel(motherboard);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, motherboard => motherboard != null);

        public ICommand DeleteMotherboardCommand => new DelegateCommand<Motherboard>(Motherboard.DeleteMotherboard, selectMotherboard => selectMotherboard != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Motherboard.RefreshCollection);
        #endregion
    }
}
