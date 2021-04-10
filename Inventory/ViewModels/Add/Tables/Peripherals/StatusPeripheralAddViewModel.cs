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

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(StatusPeripheral);
            StatusesPeripheralsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(StatusPeripheral.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
