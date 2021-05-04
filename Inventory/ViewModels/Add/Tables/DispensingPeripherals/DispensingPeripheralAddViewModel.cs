namespace Inventory.ViewModels.Add.Tables.DispensingPeripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.DispensingPeripherals;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;

    public class DispensingPeripheralAddViewModel : BindableBase
    {
        public DispensingPeripheralAddViewModel()
        {
            using var db = new InventoryEntities();

            InventoryPeripherals = new ObservableCollection<Inventory_numbers_peripherals>(db.Inventory_numbers_peripherals.AsNoTracking()
                    .Include(status => status.Statuses_peripherals)
                    .Include(peripheral => peripheral.Peripheral)
                    .Include(manufacturer => manufacturer.Peripheral.Manufacturer)
                    .Include(typePeripheral => typePeripheral.Peripheral.Types_peripherals))
                .Sort(numberPeripheral => numberPeripheral.Inventory_number);
            Employees = new ObservableCollection<Employee>(db.Employees.AsNoTracking()).Sort(emp => emp.L_name);

            for (int i = InventoryPeripherals.Count - 1; i >= 0; i--)
            {
                foreach (var peripherals in db.List_dispensed_peripherals.AsNoTracking())
                {
                    if (InventoryPeripherals[i].Id_inventory_number_peripheral == peripherals.Fk_inventory_number_peripheral)
                    {
                        InventoryPeripherals.Remove(InventoryPeripherals[i]);
                        break;
                    }
                }
            }
        }

        #region Свойства

        public Inventory_numbers_peripherals SelectInventoryPeripheral { get; set; }

        public Inventory_numbers_peripherals SelectListPeripheral { get; set; }

        public Dispensing_peripherals DispensingPeripherals { get; set; } = new();

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<Inventory_numbers_peripherals> InventoryPeripherals { get; set; }

        public ObservableCollection<Inventory_numbers_peripherals> ListDispensedPeripherals { get; set; } = new();

        #endregion

        public void OnMouseLeftButtonDownListPeripherals(object sender, RoutedEventArgs args) => SelectInventoryPeripheral = null;

        public void OnMouseLeftButtonDownListDispensing(object sender, RoutedEventArgs args) => SelectListPeripheral = null;

        #region Команды

        public ICommand TransferInListDispensingCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectPeripheral =>
        {
            ListDispensedPeripherals.Add(selectPeripheral);
            InventoryPeripherals.Remove(selectPeripheral);
        }, selectPeripheral => selectPeripheral != null);

        public ICommand TransferInListInventoryPeripheralCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectPeripheral =>
        {
            InventoryPeripherals.Add(selectPeripheral);
            ListDispensedPeripherals.Remove(selectPeripheral);
        }, selectPeripheral => selectPeripheral != null);

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(DispensingPeripherals);

            foreach (var item in ListDispensedPeripherals)
            {
                var newListDispensed = new List_dispensed_peripherals
                {
                    Fk_number_dispensing = DispensingPeripherals.Id_dispensing_peripheral,
                    Fk_inventory_number_peripheral = item.Id_inventory_number_peripheral
                };
                Services.Add(newListDispensed);
            }

            DispensingPeripheralsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => ListDispensedPeripherals.Count > 0);

        #endregion
    }
}
