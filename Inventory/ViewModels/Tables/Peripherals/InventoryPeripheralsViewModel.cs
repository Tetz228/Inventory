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

    using Inventory.Services;

    internal class InventoryPeripheralsViewModel : BindableBase
    {
        public InventoryPeripheralsViewModel()
        {
            using var db = new InventoryEntities();

            InventoryNumbersPeripherals = new ObservableCollection<Inventory_numbers_peripherals>(db.Inventory_numbers_peripherals
                .Include(status => status.Statuses_peripherals)
                .Include(peripheral => peripheral.Peripheral)
                .Include(manufacturer => manufacturer.Peripheral.Manufacturer)
                .Include(typePeripheral => typePeripheral.Peripheral.Types_peripherals));
            InventoryNumbersPeripherals.Sort(numberPeripheral => numberPeripheral.Inventory_number, SortDirection = ListSortDirection.Ascending);
            InventoryNumbersPeripheralsCollection = CollectionViewSource.GetDefaultView(InventoryNumbersPeripherals);
        }

        #region Свойства

        private ICollectionView InventoryNumbersPeripheralsCollection { get; }

        private ListSortDirection SortDirection { get; set; }

        public static ObservableCollection<Inventory_numbers_peripherals> InventoryNumbersPeripherals { get; set; }

        public Inventory_numbers_peripherals SelectInventoryNumberPeripheral { get; set; }

        private string _inventoryNumbersPeripheralsFilter = string.Empty;

        public string InventoryNumbersPeripheralsFilter
        {
            get => _inventoryNumbersPeripheralsFilter;
            set
            {
                _inventoryNumbersPeripheralsFilter = value;
                InventoryNumbersPeripheralsCollection.Filter = obj =>
                {
                    if (obj is Inventory_numbers_peripherals inventoryNumberPeripheral)
                        return inventoryNumberPeripheral.Search(InventoryNumbersPeripheralsFilter);

                    return false;
                };
                InventoryNumbersPeripheralsCollection.Refresh();
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
                                InventoryNumbersPeripherals.Sort(numberPeripheral => numberPeripheral.Inventory_number, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersPeripherals.Sort(numberPeripheral => numberPeripheral.Inventory_number, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Производитель":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersPeripherals.Sort(manufacturer => manufacturer.Peripheral.Manufacturer.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersPeripherals.Sort(manufacturer => manufacturer.Peripheral.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Тип":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersPeripherals.Sort(type => type.Peripheral.Types_peripherals.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersPeripherals.Sort(type => type.Peripheral.Types_peripherals.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersPeripherals.Sort(peripheral => peripheral.Peripheral.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersPeripherals.Sort(peripheral => peripheral.Peripheral.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Статус":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryNumbersPeripherals.Sort(peripheral => peripheral.Statuses_peripherals.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryNumbersPeripherals.Sort(peripheral => peripheral.Statuses_peripherals.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        public void OnMouseLeftButtonDown(object sender, RoutedEventArgs args) => SelectInventoryNumberPeripheral = null;
        #endregion

        #region Команды
        public ICommand AddInventoryNumberPeripheralCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryPeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryNumberPeripheralCommand => new DelegateCommand<Inventory_numbers_peripherals>(inventoryNumberPeripheral =>
        {
            var editWindow = new InventoryPeripheralEditWindow();
            var editViewModel = new InventoryPeripheralEditViewModel(inventoryNumberPeripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, peripheral => peripheral != null);

        public ICommand DeleteInventoryNumberPeripheralCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectInventoryNumberPeripheral =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить - {selectInventoryNumberPeripheral.Inventory_number}, {selectInventoryNumberPeripheral.Peripheral.Manufacturer.Name} {selectInventoryNumberPeripheral.Peripheral.Types_peripherals.Name} {selectInventoryNumberPeripheral.Peripheral.Name}?", "Удаление инвенторной периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            Services.Delete<Inventory_numbers_peripherals>(selectInventoryNumberPeripheral.Id_inventory_number_peripheral);
            RefreshCollection();
        }, selectInventoryNumberPeripheral => selectInventoryNumberPeripheral != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryNumbersPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_peripherals
                .Include(status => status.Statuses_peripherals)
                .Include(peripheral => peripheral.Peripheral)
                .Include(manufacturer => manufacturer.Peripheral.Manufacturer)
                .Include(typePeripheral => typePeripheral.Peripheral.Types_peripherals))
            {
                InventoryNumbersPeripherals.Add(item);
            }
        }
    }
}
