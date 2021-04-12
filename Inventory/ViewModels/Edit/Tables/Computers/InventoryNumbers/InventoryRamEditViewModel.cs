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

    public class InventoryRamEditViewModel : BindableBase
    {
        public InventoryRamEditViewModel(Inventory_numbers_ram inventoryRam)
        {
            using var db = new InventoryEntities();

            Rams = new ObservableCollection<Ram>(db.Rams.AsNoTracking().Include(manufacturer => manufacturer.Manufacturer).Include(type => type.Types_memory).Include(unit => unit.Unit));
            InventoryRam = inventoryRam;
            InventoryRam.BeginEdit();
        }

        public Inventory_numbers_ram InventoryRam { get; }

        public ObservableCollection<Ram> Rams { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryRam.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryRam.EndEdit();
            Services.Edit(InventoryRam.Id_inventory_number_ram, InventoryRam);
            InventoryRamViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryRam.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryRam.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
