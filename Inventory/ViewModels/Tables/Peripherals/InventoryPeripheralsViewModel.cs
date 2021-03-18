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

    class InventoryPeripheralsViewModel : BindableBase
    {
        public InventoryPeripheralsViewModel()
        {
            using var db = new InventoryEntities();

            InventoryNumbersPeripherals = new ObservableCollection<Inventory_numbers_peripherals>(db.Inventory_numbers_peripherals
                .Include(status => status.Statuses_peripherals)
                .Include(peripheral => peripheral.Peripheral)
                .Include(manufacturer => manufacturer.Peripheral.Manufacturer)
                .Include(typePeripheral => typePeripheral.Peripheral.Types_peripherals));
            InventoryNumbersPeripheralsCollection = CollectionViewSource.GetDefaultView(InventoryNumbersPeripherals);
            InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Inventory_number), ListSortDirection.Ascending));
        }

        #region Свойства
        public static ObservableCollection<Inventory_numbers_peripherals> InventoryNumbersPeripherals { get; set; }

        public ICollectionView InventoryNumbersPeripheralsCollection { get; }

        public Peripheral SelectInventoryNumberPeripheral { get; set; }

        private string _inventoryNumbersPeripheralsFilter = string.Empty;

        public string InventoryNumbersPeripheralsFilter
        {
            get => _inventoryNumbersPeripheralsFilter;
            set
            {
                _inventoryNumbersPeripheralsFilter = value;
                InventoryNumbersPeripheralsCollection.Filter = obj =>
                {
                    if (obj is Peripheral peripheral)
                        return Peripheral.Search(peripheral, InventoryNumbersPeripheralsFilter);

                    return false;
                };
                InventoryNumbersPeripheralsCollection.Refresh();
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
                case "Инвентарный номер периферии":
                {
                    if (InventoryNumbersPeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                    {
                        InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                        InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Inventory_number), ListSortDirection.Descending));
                    }
                    else
                    {
                        InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                        InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Inventory_number), ListSortDirection.Ascending));
                    }
                    InventoryNumbersPeripheralsCollection.Refresh();
                    break;
                }
                case "Тип периферии":
                    {
                        if (InventoryNumbersPeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Peripheral.Types_peripherals.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Peripheral.Types_peripherals.Name), ListSortDirection.Ascending));
                        }
                        InventoryNumbersPeripheralsCollection.Refresh();
                        break;
                    }
                case "Наименование":
                    {
                        if (InventoryNumbersPeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Peripheral.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Peripheral.Name), ListSortDirection.Ascending));
                        }
                        InventoryNumbersPeripheralsCollection.Refresh();
                        break;
                    }
                case "Производитель":
                    {
                        if (InventoryNumbersPeripheralsCollection.SortDescriptions[0].Direction == ListSortDirection.Ascending)
                        {
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Peripheral.Manufacturer.Name), ListSortDirection.Descending));
                        }
                        else
                        {
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Clear();
                            InventoryNumbersPeripheralsCollection.SortDescriptions.Add(new SortDescription(nameof(Inventory_numbers_peripherals.Peripheral.Manufacturer.Name), ListSortDirection.Ascending));
                        }
                        InventoryNumbersPeripheralsCollection.Refresh();
                        break;
                    }
            }
        }

        public void LeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryNumberPeripheral = null;
        #endregion

        #region Команды
        public ICommand AddInventoryNumberPeripheral => new DelegateCommand(() =>
        {
            var addWindow = new PeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryNumberPeripheral => new DelegateCommand<Peripheral>(peripheral =>
        {
            var editWindow = new PeripheralEditWindow();
            var editViewModel = new PeripheralEditViewModel(peripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, peripheral => peripheral != null);

        public ICommand DeleteInventoryNumberPeripheral => new DelegateCommand<Peripheral>(Peripheral.DeletePeripheral, selectPeripheral => selectPeripheral != null);

        public ICommand RefreshList => new DelegateCommand(Peripheral.RefreshCollection);
        #endregion
    }
}
