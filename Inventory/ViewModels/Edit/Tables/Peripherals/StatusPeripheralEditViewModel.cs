namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class StatusPeripheralEditViewModel : BindableBase
    {
        public StatusPeripheralEditViewModel(Statuses_peripherals statusPeripheral)
        {
            StatusPeripheral = statusPeripheral;
            StatusPeripheral.BeginEdit();
        }

        public Statuses_peripherals StatusPeripheral { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => StatusPeripheral.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            StatusPeripheral.EndEdit();
            Statuses_peripherals.EditStatusPeripheral(StatusPeripheral);
            editWindow.Close();
        }, _ => StatusPeripheral.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            StatusPeripheral.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
