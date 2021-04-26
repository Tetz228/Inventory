namespace Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    using Services;

    public class InventoryPowerSupplyEditViewModel : BindableBase
    {
        public InventoryPowerSupplyEditViewModel(Inventory_numbers_power_supplies inventoryPowerSupply)
        {
            using var db = new InventoryEntities();

            PowerSupplies = new ObservableCollection<Power_supplies>(db.Power_supplies.AsNoTracking()
                    .Include(manufacturer => manufacturer.Manufacturer)
                    .Include(unit => unit.Unit))
                    .Sort(manufact => manufact.Manufacturer.Name);
            InventoryPowerSupply = inventoryPowerSupply;
            InventoryPowerSupply.BeginEdit();
        }

        public Inventory_numbers_power_supplies InventoryPowerSupply { get; }

        public ObservableCollection<Power_supplies> PowerSupplies { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryPowerSupply.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryPowerSupply.EndEdit();
            Services.Edit(InventoryPowerSupply.Id_inventory_number_power_supplie, InventoryPowerSupply);
            InventoryPowerSupplyViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryPowerSupply.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryPowerSupply.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
