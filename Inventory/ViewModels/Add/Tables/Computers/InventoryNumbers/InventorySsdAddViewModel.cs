namespace Inventory.ViewModels.Add.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;
    using Services;

    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    public class InventorySsdAddViewModel : BindableBase
    {
        public InventorySsdAddViewModel()
        {
            using var db = new InventoryEntities();
            InventorySsd = new Inventory_numbers_ssd();
            Ssds = new ObservableCollection<Ssd>(db.Ssds.Include(manufacturer => manufacturer.Manufacturer).Include(type => type.Types_ssd).Include(unit => unit.Unit));
            InventorySsd.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_ssd InventorySsd { get; }

        public ObservableCollection<Ssd> Ssds { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_ssd.FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_ssd.Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventorySsd);
            InventorySsdViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventorySsd.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
