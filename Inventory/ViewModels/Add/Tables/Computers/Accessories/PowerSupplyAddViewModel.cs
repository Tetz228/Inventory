namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class PowerSupplyAddViewModel : BindableBase
    {
        public PowerSupplyAddViewModel()
        {
            using var db = new InventoryEntities();

            PowerSupply = new Power_supplies();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Units = new ObservableCollection<Unit>(db.Units);
        }

        public Power_supplies PowerSupply { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(PowerSupply);
            PowerSuppliesViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(PowerSupply.ErrorCollection, PowerSupply.Fk_manufacturer, PowerSupply.Fk_unit, PowerSupply.Power));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
