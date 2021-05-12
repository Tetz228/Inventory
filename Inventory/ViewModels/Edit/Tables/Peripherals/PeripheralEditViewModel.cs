namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;

    public class PeripheralEditViewModel : BindableBase, IEditableObject
    {
        public PeripheralEditViewModel(Peripheral peripheral)
        {
            using var db = new InventoryEntities();

            Peripheral = peripheral;
            BeginEdit();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            TypesPeripherals = new ObservableCollection<Types_peripherals>(db.Types_peripherals.AsNoTracking()).Sort(type => type.Name);
        }

        #region Свойства
        public Peripheral Peripheral { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_peripherals> TypesPeripherals { get; }
        #endregion

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Peripheral.Id_peripheral, Peripheral);
            PeripheralsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Peripheral.ErrorCollection));

        #region Откат изменений
        private Peripheral _selectPeripheral;

        public void BeginEdit()
        {
            _selectPeripheral = new Peripheral
            {
                Id_peripheral = Peripheral.Id_peripheral,
                Fk_manufacturer = Peripheral.Fk_manufacturer,
                Fk_type_peripheral = Peripheral.Fk_type_peripheral,
                Name = Peripheral.Name
            };
        }

        public void EndEdit() => _selectPeripheral = null;

        public void CancelEdit()
        {
            if (_selectPeripheral == null)
                return;

            Peripheral.Id_peripheral = _selectPeripheral.Id_peripheral;
            Peripheral.Fk_manufacturer = _selectPeripheral.Fk_manufacturer;
            Peripheral.Fk_type_peripheral = _selectPeripheral.Fk_type_peripheral;
            Peripheral.Name = _selectPeripheral.Name;
        }
        #endregion
    }
}
