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

    public class InventoryProcessorAddViewModel : BindableBase
    {
        public InventoryProcessorAddViewModel()
        {
            using var db = new InventoryEntities();

            Processors = new ObservableCollection<Processor>(db.Processors.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(unit => unit.Unit)
                .Include(socket => socket.Socket))
                .Sort(socket => socket.Socket.Name);

            try
            {
                InventoryProcessor.Inventory_number = db.Inventory_numbers_processors.Select(processors => processors.Inventory_number).Max() + 1;
            }
            catch
            {
                InventoryProcessor.Inventory_number = 1;
            }
        }

        public Inventory_numbers_processors InventoryProcessor { get; } = new();

        public ObservableCollection<Processor> Processors { get; }

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
