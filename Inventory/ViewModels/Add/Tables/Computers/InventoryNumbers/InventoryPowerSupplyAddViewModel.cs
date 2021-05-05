namespace Inventory.ViewModels.Add.Tables.Computers.InventoryNumbers
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;
    using Services;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    public class InventoryPowerSupplyAddViewModel : BindableBase
    {
        public InventoryPowerSupplyAddViewModel()
        {
            using var db = new InventoryEntities();

            PowerSupplies = new ObservableCollection<Power_supplies>(db.Power_supplies.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(unit => unit.Unit))
                .Sort(manufact => manufact.Manufacturer.Name);

            try
            {
                InventoryPowerSupply.Inventory_number = db.Inventory_numbers_power_supplies.Select(powerSupplies => powerSupplies.Inventory_number).Max() + 1;
            }
            catch
            {
                InventoryPowerSupply.Inventory_number = 1;
            }
        }

        public Inventory_numbers_power_supplies InventoryPowerSupply { get; } = new();

        public ObservableCollection<Power_supplies> PowerSupplies { get; }
        
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryPowerSupply);
            InventoryPowerSuppliesViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryPowerSupply.ErrorCollection));
    }
}
