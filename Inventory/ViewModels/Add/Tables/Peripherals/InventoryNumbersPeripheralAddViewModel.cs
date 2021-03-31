namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;

    public class InventoryNumbersPeripheralAddViewModel : BindableBase
    {
        public InventoryNumbersPeripheralAddViewModel()
        {
            using var db = new InventoryEntities();

            InventoryNumberPeripheral = new Inventory_numbers_peripherals();
            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals.Include(manufacturer=>manufacturer.Manufacturer).Include(type => type.Types_peripherals));
            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals);
            InventoryNumberPeripheral.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_peripherals InventoryNumberPeripheral { get; }

        public ObservableCollection<Peripheral> Peripherals { get; }

        public ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_peripherals.FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_peripherals.Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryNumberPeripheral);
            InventoryNumbersPeripheralViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryNumberPeripheral.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}