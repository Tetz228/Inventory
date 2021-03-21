namespace Inventory.ViewModels.Tables
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.View.Add.Tables;
    using Inventory.View.Edit.Tables;
    using Inventory.ViewModels.Edit;

    public class ManufacturersViewModel : BindableBase
    {
        public ManufacturersViewModel()
        {
            using var db = new InventoryEntities();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Manufacturers.Sort(manufacturer => manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            ManufacturersCollection = CollectionViewSource.GetDefaultView(Manufacturers);
        }

        #region Свойства
        public ICollectionView ManufacturersCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Manufacturer> Manufacturers { get; set; }

        public Manufacturer SelectManufacturer { get; set; }

        private string _manufacturersFilter = string.Empty;

        public string ManufacturersFilter
        {
            get => _manufacturersFilter;
            set
            {
                _manufacturersFilter = value;
                ManufacturersCollection.Filter = obj =>
                {
                    if (obj is Manufacturer manufacturer)
                        return Manufacturer.Search(manufacturer, ManufacturersFilter);

                    return false;
                };
                ManufacturersCollection.Refresh();
            }
        }
        #endregion

        #region События
        public void Sort(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is not GridViewColumnHeader columnHeader)
                return;

            switch (columnHeader.Content.ToString())
            {
                case "Наименование":
                {
                    if (SortDirection == ListSortDirection.Ascending)
                        Manufacturers.Sort(manufacturer => manufacturer.Name, SortDirection = ListSortDirection.Descending);
                    else
                        Manufacturers.Sort(manufacturer => manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                    break;
                }
            }
        }
        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectManufacturer = null;
        #endregion

        #region Команды
        public ICommand AddManufacturer => new DelegateCommand(() =>
        {
            var addManufacturerWindow = new ManufacturerAddWindow();
            addManufacturerWindow.ShowDialog();
        });

        public ICommand EditManufacturer => new DelegateCommand<Manufacturer>(manufacturer =>
        {
            var editWindow = new ManufacturerEditWindow();
            var editViewModel = new ManufacturerEditViewModel(manufacturer);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, manufacturer => manufacturer != null);

        public ICommand DeleteManufacturer => new DelegateCommand<Manufacturer>(Manufacturer.DeleteManufacturer, selectManufacturer => selectManufacturer != null);

        public ICommand RefreshList => new DelegateCommand(Manufacturer.RefreshCollection);
        #endregion
    }
}