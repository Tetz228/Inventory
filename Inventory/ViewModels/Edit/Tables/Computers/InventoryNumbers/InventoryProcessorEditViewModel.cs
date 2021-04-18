namespace Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;
    using Services;
    using DevExpress.Mvvm;

    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    public class InventoryProcessorEditViewModel : BindableBase
    {
        public InventoryProcessorEditViewModel(Inventory_numbers_processors inventoryProcessor)
        {
            using var db = new InventoryEntities();

            Processors = new ObservableCollection<Processor>(db.Processors.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(unit => unit.Unit).Include(socket => socket.Socket));
            InventoryProcessor = inventoryProcessor;
            InventoryProcessor.BeginEdit();
        }

        public Inventory_numbers_processors InventoryProcessor { get; }

        public ObservableCollection<Processor> Processors { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryProcessor.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryProcessor.EndEdit();
            Services.Edit(InventoryProcessor.Id_inventory_number_processor, InventoryProcessor);
            InventoryProcessorViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryProcessor.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryProcessor.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
