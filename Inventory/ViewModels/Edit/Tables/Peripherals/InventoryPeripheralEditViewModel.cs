namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;

    public class InventoryPeripheralEditViewModel : BindableBase
    {
        public InventoryPeripheralEditViewModel(Inventory_numbers_peripherals inventoryPeripheral)
        {
            using var db = new InventoryEntities();

            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals.AsNoTracking()
                    .Include(manufacturer => manufacturer.Manufacturer)
                    .Include(type => type.Types_peripherals))
                    .Sort(manufact => manufact.Manufacturer.Name);
            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals.AsNoTracking()).Sort(status => status.Name);

            InventoryPeripheral = inventoryPeripheral;
            InventoryPeripheral.BeginEdit();
        }

        public Inventory_numbers_peripherals InventoryPeripheral { get; }

        public ObservableCollection<Peripheral> Peripherals { get; }

        public ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryPeripheral.CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryPeripheral.EndEdit();
            Services.Edit(InventoryPeripheral.Id_inventory_number_peripheral, InventoryPeripheral);
            InventoryPeripheralViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryPeripheral.ErrorCollection));
    }
}
