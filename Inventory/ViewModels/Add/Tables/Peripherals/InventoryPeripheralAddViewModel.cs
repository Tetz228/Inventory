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

            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(type => type.Types_peripherals));
            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals.AsNoTracking());
            InventoryPeripheral.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_peripherals InventoryPeripheral { get; } = new();

        public ObservableCollection<Peripheral> Peripherals { get; }

        public ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_peripherals.AsNoTracking().FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_peripherals.AsNoTracking().Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

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