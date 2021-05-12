namespace Inventory.ViewModels.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.View.Add.Tables.Peripherals;
    using Inventory.View.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Edit.Tables.Peripherals;
    using Inventory.ViewModels.Tables.Base;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class InventoryPeripheralViewModel : BaseViewModel<Inventory_numbers_peripherals>
    {
        private const string NAME_TEMPLATE = "Шаблон для инвентаризации периферии.xlsx";
        private const string NAMED_AREA_NAME = "InventoryPeripherals";

        public InventoryPeripheralViewModel() : base(InventoryPeripherals, NAME_TEMPLATE, NAMED_AREA_NAME) => RefreshCollection();
        
        public static ObservableCollection<Inventory_numbers_peripherals> InventoryPeripherals { get; set; } = new();

        public override void GridViewColumnHeader_OnClick(object sender, RoutedEventArgs args)
        {
            if (args.OriginalSource is GridViewColumnHeader columnHeader && columnHeader.Content != null)
            {
                switch (columnHeader.Content.ToString())
                {
                    case "Инвентарный номер":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryPeripherals.Sort(numberPeripheral => numberPeripheral.Inventory_number, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryPeripherals.Sort(numberPeripheral => numberPeripheral.Inventory_number, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Производитель":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryPeripherals.Sort(manufacturer => manufacturer.Peripheral.Manufacturer.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryPeripherals.Sort(manufacturer => manufacturer.Peripheral.Manufacturer.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Тип":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryPeripherals.Sort(type => type.Peripheral.Types_peripherals.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryPeripherals.Sort(type => type.Peripheral.Types_peripherals.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Наименование":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryPeripherals.Sort(peripheral => peripheral.Peripheral.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryPeripherals.Sort(peripheral => peripheral.Peripheral.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                    case "Статус":
                        {
                            if (SortDirection == ListSortDirection.Ascending)
                                InventoryPeripherals.Sort(peripheral => peripheral.Statuses_peripherals.Name, SortDirection = ListSortDirection.Descending);
                            else
                                InventoryPeripherals.Sort(peripheral => peripheral.Statuses_peripherals.Name, SortDirection = ListSortDirection.Ascending);
                            break;
                        }
                }
            }
        }

        #region Команды
        public ICommand AddInventoryPeripheralCommand => new DelegateCommand(() =>
        {
            var addWindow = new InventoryPeripheralAddWindow();
            addWindow.ShowDialog();
        });

        public ICommand EditInventoryPeripheralCommand => new DelegateCommand<Inventory_numbers_peripherals>(inventoryPeripheral =>
        {
            var editWindow = new InventoryPeripheralEditWindow();
            var editViewModel = new InventoryPeripheralEditViewModel(inventoryPeripheral);
            editWindow.DataContext = editViewModel;
            editWindow.Closing += editViewModel.OnWindowClosing;
            editWindow.ShowDialog();
        }, inventoryPeripheral => inventoryPeripheral != null);

        public ICommand DeleteInventoryPeripheralCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectInventoryPeripheral =>
        {
            var messageResult = MessageBox.Show($"Вы действительно хотите удалить инвентарную периферию:\nинвент. номер - {selectInventoryPeripheral.Inventory_number};\nпроизводитель - {selectInventoryPeripheral.Peripheral.Manufacturer.Name};\nтип - {selectInventoryPeripheral.Peripheral.Types_peripherals.Name};\nнаименование - {selectInventoryPeripheral.Peripheral.Name}?", "Удаление инвентарной периферии", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (messageResult != MessageBoxResult.Yes)
                return;

            if(Services.Delete<Inventory_numbers_peripherals>(selectInventoryPeripheral.Id_inventory_number_peripheral))
               InventoryPeripherals.Remove(selectInventoryPeripheral);
        }, selectInventoryPeripheral => selectInventoryPeripheral != null);

        public ICommand RefreshCollectionCommand => new DelegateCommand(RefreshCollection);
        #endregion

        public static void RefreshCollection()
        {
            InventoryPeripherals.Clear();
            using var db = new InventoryEntities();

            foreach (var item in db.Inventory_numbers_peripherals.AsNoTracking()
                .Include(status => status.Statuses_peripherals)
                .Include(peripheral => peripheral.Peripheral)
                .Include(manufacturer => manufacturer.Peripheral.Manufacturer)
                .Include(typePeripheral => typePeripheral.Peripheral.Types_peripherals))
            {
                InventoryPeripherals.Add(item);
            }
            InventoryPeripherals.Sort(numberPeripheral => numberPeripheral.Inventory_number);
        }
    }
}
