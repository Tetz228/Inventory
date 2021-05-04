namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;
    using System.Windows;
    using System.Windows.Input;

    public class TypePeripheralAddViewModel : BindableBase
    {
        public Types_peripherals TypePeripheral { get; } = new();

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(TypePeripheral);
            TypesPeripheralsViewModel.TypesPeripherals.Add(TypePeripheral);
            addWindow.Close();
        }, _ => Services.IsValidationProperties(TypePeripheral.ErrorCollection));
    }
}
