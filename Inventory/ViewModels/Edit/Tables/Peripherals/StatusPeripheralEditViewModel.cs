namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class StatusPeripheralEditViewModel : BindableBase, IEditableObject
    {
        public StatusPeripheralEditViewModel(Statuses_peripherals statusPeripheral)
        {
            StatusPeripheral = statusPeripheral;
            BeginEdit();
        }

        public Statuses_peripherals StatusPeripheral { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(StatusPeripheral.Id_status_peripheral, StatusPeripheral);
            editWindow.Close();
        }, _ => Services.IsValidationProperties(StatusPeripheral.ErrorCollection));

        #region Откат изменений
        private Statuses_peripherals _selectStatusPeripheral;

        public void BeginEdit()
        {
            _selectStatusPeripheral = new Statuses_peripherals
            {
                Id_status_peripheral = StatusPeripheral.Id_status_peripheral,
                Name = StatusPeripheral.Name
            };
        }

        public void EndEdit()
        {
            _selectStatusPeripheral = null;
        }

        public void CancelEdit()
        {
            if (_selectStatusPeripheral == null)
                return;

            StatusPeripheral.Id_status_peripheral = _selectStatusPeripheral.Id_status_peripheral;
            StatusPeripheral.Name = _selectStatusPeripheral.Name;
        }
        #endregion
    }
}
