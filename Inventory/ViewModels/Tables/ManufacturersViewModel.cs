namespace Inventory.ViewModels.Tables
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables;
    using Inventory.View.Edit.Tables;
    using Inventory.ViewModels.Edit;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class ManufacturersViewModel : BaseViewModel<Manufacturer>
    {
        public ManufacturersViewModel() : base(Manufacturers) => RefreshCollection();

        public static ObservableCollection<Manufacturer> Manufacturers { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
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

            if (Services.Delete<Manufacturer>(selectManufacturer.Id_manufacturer))
                Manufacturers.Remove(selectManufacturer);
        }, selectManufacturer => selectManufacturer != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        
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