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

    public class InventoryHddAddViewModel : BindableBase
    {
        public InventoryHddAddViewModel()
        {
            using var db = new InventoryEntities();

            Hdd = new ObservableCollection<Hdd>(db.Hdds.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(type => type.Types_hdd)
                .Include(unit => unit.Unit))
                .Sort(manufact => manufact.Manufacturer.Name);

            InventoryHdd.Inventory_number = db.Inventory_numbers_hdd.FirstOrDefault() == null ? InventoryHdd.Inventory_number = 1 : InventoryHdd.Inventory_number = db.Inventory_numbers_hdd.Select(hdd => hdd.Inventory_number).Max() + 1;
        }

        public Inventory_numbers_hdd InventoryHdd { get; } = new();

        public ObservableCollection<Hdd> Hdd { get; }

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryHdd);
            InventoryHddViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryHdd.ErrorCollection));
    }
}
