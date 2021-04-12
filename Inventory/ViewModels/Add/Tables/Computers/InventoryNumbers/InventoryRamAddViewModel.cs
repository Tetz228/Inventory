namespace Inventory.ViewModels.Add.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using Services;
    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    public class InventoryRamAddViewModel : BindableBase
    {
        public InventoryRamAddViewModel()
        {
            using var db = new InventoryEntities();

            Rams = new ObservableCollection<Ram>(db.Rams.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(type => type.Types_memory));
            InventoryRam.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_ram InventoryRam { get; } = new();

        public ObservableCollection<Ram> Rams { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_ram.AsNoTracking().FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_ram.AsNoTracking().Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

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
