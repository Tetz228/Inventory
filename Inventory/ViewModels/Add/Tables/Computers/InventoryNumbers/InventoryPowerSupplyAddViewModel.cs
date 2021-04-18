namespace Inventory.ViewModels.Add.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    using Services;
    public class InventoryPowerSupplyAddViewModel : BindableBase
    {
        public InventoryPowerSupplyAddViewModel()
        {
            using var db = new InventoryEntities();

            PowerSupplies = new ObservableCollection<Power_supplies>(db.Power_supplies.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit));
            InventoryPowerSupply.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_power_supplies InventoryPowerSupply { get; } = new();

        public ObservableCollection<Power_supplies> PowerSupplies { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_power_supplies.AsNoTracking().FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_power_supplies.AsNoTracking().Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryPowerSupply);
            InventoryPowerSupplyViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryPowerSupply.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
