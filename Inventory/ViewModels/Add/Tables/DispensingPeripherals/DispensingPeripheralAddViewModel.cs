namespace Inventory.ViewModels.Add.Tables.DispensingPeripherals
{
    using System;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.ViewModels.Tables.DispensingPeripherals;

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
        }

        public Dispensing_peripherals DispensingPeripherals { get; set; } = new();

        public Inventory_numbers_peripherals SelectInventoryPeripheral { get; set; }

        public Inventory_numbers_peripherals SelectListPeripheral { get; set; }

        public ObservableCollection<Employee> Employees { get; }

        public ObservableCollection<Inventory_numbers_peripherals> InventoryPeripherals { get; set; } = new();

        public ObservableCollection<Inventory_numbers_peripherals> ListDispensing { get; set; } = new();

        public void OnMouseLeftButtonDownListPeripherals(object sender, RoutedEventArgs args) => SelectInventoryPeripheral = null;

        public void OnMouseLeftButtonDownListDispensing(object sender, RoutedEventArgs args) => SelectListPeripheral = null;

        public ICommand TransferInListDispensingCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectPeripheral =>
        {
            ListDispensing.Add(selectPeripheral);
            InventoryPeripherals.Remove(selectPeripheral);
        }, selectPeripheral => selectPeripheral != null);

        public ICommand TransferInListInventoryPeripheralCommand => new DelegateCommand<Inventory_numbers_peripherals>(selectPeripheral =>
        {
            InventoryPeripherals.Add(selectPeripheral);
            ListDispensing.Remove(selectPeripheral);
        }, selectPeripheral => selectPeripheral != null);

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            DispensingPeripherals.Fk_user = 1/*User.AuthorizedUser.Id_user*/;
            DispensingPeripherals.Date_dispensing = DateTime.Now.Date;
            Services.Add(DispensingPeripherals);

            foreach (var item in ListDispensing)
            {
                var list = new List_dispensed_peripherals
                {
                    Fk_number_dispensing = DispensingPeripherals.Id_dispensing_peripheral,
                    Fk_inventory_number_peripheral = item.Id_inventory_number_peripheral
                };

                Services.Add(list);
            }

            DispensingPeripheralsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => ListDispensing.Count > 0);
    }
}
