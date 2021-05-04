namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class PeripheralAddViewModel : BindableBase
    {
        public PeripheralAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            TypesPeripherals = new ObservableCollection<Types_peripherals>(db.Types_peripherals.AsNoTracking()).Sort(type => type.Name);
        }

        public Peripheral Peripheral { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_peripherals> TypesPeripherals { get; }

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Peripheral);
            PeripheralsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Peripheral.ErrorCollection));
    }
}
