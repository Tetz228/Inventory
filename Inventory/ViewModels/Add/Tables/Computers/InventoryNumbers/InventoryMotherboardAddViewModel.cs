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

    public class InventoryMotherboardAddViewModel : BindableBase
    {
        public InventoryMotherboardAddViewModel()
        {
            using var db = new InventoryEntities();

            Motherboards = new ObservableCollection<Motherboard>(db.Motherboards.AsNoTracking()
                .Include(manufacturer => manufacturer.Manufacturer)
                .Include(socket => socket.Socket))
                .Sort(socket => socket.Socket.Name);

            try
            {
                InventoryMotherboard.Inventory_number = db.Inventory_numbers_motherboards.Select(motherboards => motherboards.Inventory_number).Max() + 1;
            }
            catch
            {
                InventoryMotherboard.Inventory_number = 1;
            }
        }

        public Inventory_numbers_motherboards InventoryMotherboard { get; } = new();

        public ObservableCollection<Motherboard> Motherboards { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(InventoryMotherboard);
            InventoryMotherboardViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(InventoryMotherboard.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
