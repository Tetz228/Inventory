namespace Inventory.ViewModels.Edit.Tables.Computers.InventoryNumbers
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;
    using Services;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.InventoryNumbers;

    class InventoryNumbersHddEditViewModel : BindableBase
    {
        public InventoryNumbersHddEditViewModel(Inventory_numbers_hdd inventoryNumberHdd)
        {
            using var db = new InventoryEntities();

            Hdds = new ObservableCollection<Hdd>(db.Hdds.Include(manufacturer => manufacturer.Manufacturer).Include(type => type.Types_hdd).Include(unit => unit.Unit));
            InventoryNumberHdd = inventoryNumberHdd;
            InventoryNumberHdd.InventoryNumberString = inventoryNumberHdd.Inventory_number.ToString();
            InventoryNumberHdd.BeginEdit();
        }

        public Inventory_numbers_hdd InventoryNumberHdd { get; }

        public ObservableCollection<Hdd> Hdds { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryNumberHdd.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryNumberHdd.EndEdit();
            Services.Edit(InventoryNumberHdd.Id_inventory_number_hdd, InventoryNumberHdd);
            InventoryNumbersHddViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryNumberHdd.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryNumberHdd.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
