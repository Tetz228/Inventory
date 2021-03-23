namespace Inventory.ViewModels.Tables.Computers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Computers;
    using Inventory.View.Edit.Tables.Computers;
    using Inventory.ViewModels.Edit.Tables.Computers;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class HddsViewModel : BindableBase
    {
        public HddsViewModel()
        {
            using var db = new InventoryEntities();

            Hdds = new ObservableCollection<Hdd>(db.Hdds.Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_hdd));
            Hdds.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            HddsCollection = CollectionViewSource.GetDefaultView(Hdds);
        }

        #region Свойства
        private ICollectionView HddsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public Hdd SelectHdd { get; set; }

        public static ObservableCollection<Hdd> Hdds { get; set; }

        private string _hddsFilter = string.Empty;

        public string HddsFilter
        {
            get => _hddsFilter;
            set
            {
                _hddsFilter = value;
                HddsCollection.Filter = obj =>
                {
                    if (obj is Hdd hdd)
                        return Hdd.SearchFor(hdd, HddsFilter);

                    return false;
                };
                HddsCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Производитель":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            Hdds.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Descending);
                        else
                            Hdds.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
                case "Тип":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            Hdds.Sort(type => type.Types_hdd.Name, SortDirection = ListSortDirection.Descending);
                        else
                            Hdds.Sort(type => type.Types_hdd.Name, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
                case "Наименование":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            Hdds.Sort(hdd => hdd.Name, SortDirection = ListSortDirection.Descending);
                        else
                            Hdds.Sort(hdd => hdd.Name, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
                case "Объём":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            Hdds.Sort(hdd => hdd.Memory_size, SortDirection = ListSortDirection.Descending);
                        else
                            Hdds.Sort(hdd => hdd.Memory_size, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
                case "Единица измерения":
                    {
                        if (SortDirection == ListSortDirection.Ascending)
                            Hdds.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Descending);
                        else
                            Hdds.Sort(unit => unit.Unit.Full_name, SortDirection = ListSortDirection.Ascending);
                        break;
                    }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectHdd = null;
        #endregion

        #region Команды
        public ICommand AddHddCommand => new DelegateCommand(() =>
        {
            var addHddWindow = new HddAddWindow();
            addHddWindow.ShowDialog();
        });

        public ICommand EditHddCommand => new DelegateCommand<Hdd>(hdd =>
        {
            var editWindow = new HddEditWindow();
            var editViewModel = new HddEditViewModel(hdd);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, hdd => hdd != null);

        public ICommand DeleteHddCommand => new DelegateCommand<Hdd>(Hdd.DeleteHdd, selectHdd => selectHdd != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(Hdd.RefreshCollection);
        #endregion
    }
}
