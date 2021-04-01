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

    using Inventory.Services;

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
                        return motherboard.Search(MotherboardsFilter);

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
                SortDirection = SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;

                switch (columnHeader.Content.ToString())
                {
                    case "Производитель":
                        {
                            Motherboards.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection);
                            break;
                        }
                    case "Наименование":
                        {
                            Motherboards.Sort(graphicCard => graphicCard.Name, SortDirection);
                            break;
                        }
                    case "Сокет":
                        {
                            Motherboards.Sort(socket => socket.Socket.Name, SortDirection);
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

        public ICommand DeleteMotherboardCommand => new DelegateCommand<Motherboard>(selectMotherboard =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectMotherboard.Manufacturer.Name} {selectMotherboard.Name} {selectMotherboard.Socket.Name}?", "Удаление материнской платы", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Motherboard>(selectMotherboard.Id_motherboard);
            RefreshCollection();
        }, selectMotherboard => selectMotherboard != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            Motherboards.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Motherboards.Include(manufacturer => manufacturer.Manufacturer).Include(socket => socket.Socket))
                Motherboards.Add(item);
        }
    }
}
