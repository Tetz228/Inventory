namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;

    public class StatusPeripheralAddViewModel : BindableBase
    {
        public Statuses_peripherals StatusPeripheral { get; } = new();

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(StatusPeripheral);
            StatusesPeripheralsViewModel.StatusesPeripherals.Add(StatusPeripheral);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(StatusPeripheral.ErrorCollection));
    }
}
