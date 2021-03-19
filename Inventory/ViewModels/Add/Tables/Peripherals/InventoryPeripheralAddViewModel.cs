namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    internal class InventoryPeripheralAddViewModel : BindableBase
    {
        public InventoryPeripheralAddViewModel()
        {
            using var db = new InventoryEntities();

            InventoryNumberPeripheral = new Inventory_numbers_peripherals();
            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals);
            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals);
            InventoryNumberPeripheral.Inventory_numberString = Inventory_numbers_peripherals.MaxInventoryNumber().ToString();
        }

        public Inventory_numbers_peripherals InventoryNumberPeripheral { get; }

        public ObservableCollection<Peripheral> Peripherals { get; }

        public ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Inventory_numbers_peripherals.AddInventoryNumber(InventoryNumberPeripheral);
            addWindow.Close();
        }, _ => InventoryNumberPeripheral.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}