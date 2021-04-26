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

    public class InventoryRamAddViewModel : BindableBase
    {
        public InventoryRamAddViewModel()
        {
            using var db = new InventoryEntities();

            Rams = new ObservableCollection<Ram>(db.Rams.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(unit => unit.Unit)
                .Include(type => type.Types_memory))
                .Sort(manufact => manufact.Manufacturer.Name);

            try
            {
                InventoryRam.Inventory_number = db.Inventory_numbers_ram.Select(ram => ram.Inventory_number).Max() + 1;
            }
            catch
            {
                InventoryRam.Inventory_number = 1;
            }
        }

        public Inventory_numbers_ram InventoryRam { get; } = new();

        public ObservableCollection<Ram> Rams { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryRam);
            InventoryRamViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryRam.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
