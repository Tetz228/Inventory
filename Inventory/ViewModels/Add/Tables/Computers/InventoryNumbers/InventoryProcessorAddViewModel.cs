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

    public class InventoryProcessorAddViewModel : BindableBase
    {
        public InventoryProcessorAddViewModel()
        {
            using var db = new InventoryEntities();

            Processors = new ObservableCollection<Processor>(db.Processors.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(socket => socket.Socket));
            InventoryProcessor.Inventory_number = MaxInventoryNumber();
        }

        public Inventory_numbers_processors InventoryProcessor { get; } = new();

        public ObservableCollection<Processor> Processors { get; }

        private static int MaxInventoryNumber()
        {
            using var db = new InventoryEntities();
            var isEmpty = db.Inventory_numbers_processors.AsNoTracking().FirstOrDefault();

            if (isEmpty == null)
                return 1;

            var isUniqueNumber = db.Inventory_numbers_processors.AsNoTracking().Max(number => number.Inventory_number);

            return ++isUniqueNumber;
        }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryProcessor);
            InventoryProcessorViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryProcessor.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
