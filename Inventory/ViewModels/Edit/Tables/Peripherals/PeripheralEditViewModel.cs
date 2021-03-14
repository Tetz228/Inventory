namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    class PeripheralEditViewModel : BindableBase
    {
        public PeripheralEditViewModel(Peripheral peripheral)
        {
            using var db = new InventoryEntities();

            Peripheral = peripheral;
            Peripheral.BeginEdit();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            TypesPeripherals = new ObservableCollection<Types_peripherals>(db.Types_peripherals);
        }

        #region Свойства
        public Peripheral Peripheral { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_peripherals> TypesPeripherals { get; }
        #endregion

        public void OnWindowClosing(object sender, CancelEventArgs e) => Peripheral.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Peripheral.EndEdit();
            Peripheral.EditPeripheral(Peripheral);
            editWindow.Close();
        }, _ => Peripheral.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            Peripheral.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
