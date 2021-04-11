namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class PowerSupplyEditViewModel : BindableBase, IEditableObject
    {
        public PowerSupplyEditViewModel(Power_supplies powerSupply)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking());
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking());

            PowerSupply = powerSupply;
            PowerSupply.PowerString = powerSupply.Power.ToString();
            BeginEdit();
        }

        public Power_supplies PowerSupply { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(PowerSupply.Id_power_supplie, PowerSupply);
            PowerSuppliesViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(PowerSupply.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Power_supplies _selectPowerSupply;

        public void BeginEdit()
        {
            _selectPowerSupply = new Power_supplies
            {
                Id_power_supplie = PowerSupply.Id_power_supplie,
                Power = PowerSupply.Power,
                Name = PowerSupply.Name,
                Fk_unit = PowerSupply.Fk_unit,
                Fk_manufacturer = PowerSupply.Fk_manufacturer,
            };
        }

        public void EndEdit() => _selectPowerSupply = null;

        public void CancelEdit()
        {
            if (_selectPowerSupply == null)
                return;

            PowerSupply.Id_power_supplie = _selectPowerSupply.Id_power_supplie;
            PowerSupply.Power = _selectPowerSupply.Power;
            PowerSupply.Name = _selectPowerSupply.Name;
            PowerSupply.Fk_unit = _selectPowerSupply.Fk_unit;
            PowerSupply.Fk_manufacturer = _selectPowerSupply.Fk_manufacturer;
        }
        #endregion
    }
}
