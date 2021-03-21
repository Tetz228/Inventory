namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit.Tables.Peripherals;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public class PeripheralsViewModel : BindableBase
    {
        public PeripheralsViewModel()
        {
            using var db = new InventoryEntities();

            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals.Include(manufacturer => manufacturer.Manufacturer).Include(typePeripheral => typePeripheral.Types_peripherals));
            Peripherals.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
            PeripheralsCollection = CollectionViewSource.GetDefaultView(Peripherals);
        }

        #region Свойства
        private ICollectionView PeripheralsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Peripheral> Peripherals { get; set; }

        public Peripheral SelectPeripheral { get; set; }

        private string _peripheralsFilter = string.Empty;

        public string PeripheralsFilter
        {
            get => _peripheralsFilter;
            set
            {
                _peripheralsFilter = value;
                PeripheralsCollection.Filter = obj =>
                {
                    if (obj is Peripheral peripheral)
                        return Peripheral.Search(peripheral, PeripheralsFilter);

                    return false;
                };
                PeripheralsCollection.Refresh();
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
                case "Производитель":
                {
                    if (SortDirection == ListSortDirection.Ascending)
                        Peripherals.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Descending);
                    else
                        Peripherals.Sort(manufacturer => manufacturer.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                    break;
                }
                case "Тип периферии":
                {
                    if (SortDirection == ListSortDirection.Ascending)
                        Peripherals.Sort(type => type.Types_peripherals.Name, SortDirection = ListSortDirection.Descending);
                    else
                        Peripherals.Sort(type => type.Types_peripherals.Name, SortDirection = ListSortDirection.Ascending);
                    break;
                }
                case "Наименование":
                {
                    if (SortDirection == ListSortDirection.Ascending)
                        Peripherals.Sort(peripheral => peripheral.Name, SortDirection = ListSortDirection.Descending);
                    else
                        Peripherals.Sort(peripheral => peripheral.Name, SortDirection = ListSortDirection.Ascending);
                    break;
                }
            }
        }

        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectPeripheral = null;
        #endregion

        #region Команды
        public ICommand AddPeripheral => new DelegateCommand(() =>
        {
            var addWindow = new PeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditPeripheral => new DelegateCommand<Peripheral>(peripheral =>
        {
            var editWindow = new PeripheralEditWindow();
            var editViewModel = new PeripheralEditViewModel(peripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, peripheral => peripheral != null);

        public ICommand DeletePeripheral => new DelegateCommand<Peripheral>(Peripheral.DeletePeripheral, selectPeripheral => selectPeripheral != null);

        public ICommand RefreshList => new DelegateCommand(Peripheral.RefreshCollection);
        #endregion
    }
}