namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Peripherals;

    public class StatusPeripheralAddViewModel : BindableBase
    {
        public StatusPeripheralAddViewModel() => StatusPeripheral = new Statuses_peripherals();

        public Statuses_peripherals StatusPeripheral { get; }

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
