namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class InventoryPeripheralAddViewModel : BindableBase
    {
        public InventoryPeripheralAddViewModel()
        {
            using var db = new InventoryEntities();

            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(type => type.Types_peripherals))
                .Sort(manufact => manufact.Manufacturer.Name);
            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals.AsNoTracking()).Sort(status => status.Name);

            try
            {
                InventoryPeripheral.Inventory_number = db.Inventory_numbers_peripherals.Select(peripherals => peripherals.Inventory_number).Max() + 1;
            }
            catch
            {
                InventoryPeripheral.Inventory_number = 1;
            }
        }

        public Inventory_numbers_peripherals InventoryPeripheral { get; } = new();

        public ObservableCollection<Peripheral> Peripherals { get; }

        public ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryPeripheral);
            InventoryPeripheralViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryPeripheral.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}