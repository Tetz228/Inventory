namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    public class StatusPeripheralAddViewModel : BindableBase
    {
        public StatusPeripheralAddViewModel() => StatusPeripheral = new Statuses_peripherals();

        public Statuses_peripherals StatusPeripheral { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(StatusPeripheral);
            Statuses_peripherals.RefreshCollection();
            addWindow.Close();
        }, _ => StatusPeripheral.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
