namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using System.Collections.ObjectModel;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class PeripheralAddViewModel : BindableBase
    {
        public PeripheralAddViewModel()
        {
            using var db = new InventoryEntities();

            Peripheral = new Peripheral();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            TypesPeripherals = new ObservableCollection<Types_peripherals>(db.Types_peripherals);
        }

        public Peripheral Peripheral { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_peripherals> TypesPeripherals { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Peripheral.AddPeripheral(Peripheral);
            addWindow.Close();
        }, _ => Peripheral.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
