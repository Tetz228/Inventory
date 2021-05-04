namespace Inventory.ViewModels.Tables
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables;
    using Inventory.View.Edit.Tables;
    using Inventory.ViewModels.Edit;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Inventory.Services;

    public class ManufacturersViewModel : BindableBase
    {
        public ManufacturersViewModel()
        {
            RefreshCollection();
            ManufacturersCollection = CollectionViewSource.GetDefaultView(Manufacturers);
        }

        #region Свойства
        public ICollectionView ManufacturersCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Manufacturer> Manufacturers { get; set; } = new();

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
                        return manufacturer.Search(ManufacturersFilter);

                    return false;
                };
                ManufacturersCollection.Refresh();
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
        }
        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectManufacturer = null;
        #endregion

        #region Команды
        public ICommand AddManufacturerCommand => new DelegateCommand(() =>
        {
            var addManufacturerWindow = new ManufacturerAddWindow();
            addManufacturerWindow.ShowDialog();
        });

        public ICommand EditManufacturerCommand => new DelegateCommand<Manufacturer>(manufacturer =>
        {
            var editWindow = new ManufacturerEditWindow();
            var editViewModel = new ManufacturerEditViewModel(manufacturer);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, manufacturer => manufacturer != null);

        public ICommand DeleteManufacturerCommand => new DelegateCommand<Manufacturer>(selectManufacturer =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить производителя:\nнаименование - {selectManufacturer.Name}?", "Удаление производителя", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Manufacturer>(selectManufacturer.Id_manufacturer);
            Manufacturers.Remove(selectManufacturer);
        }, selectManufacturer => selectManufacturer != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        private static void RefreshCollection()
        {
            Manufacturers.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Manufacturers.AsNoTracking())
            {
                Manufacturers.Add(item);
            }
            Manufacturers.Sort(manufacturer => manufacturer.Name);
        }
    }
}