namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Inventory.Services;
    using System.Windows.Input;

    public class RamAddViewModel : BindableBase
    {
        public RamAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking()).Sort(unit => unit.Full_name);
            TypesMemories = new ObservableCollection<Types_memory>(db.Types_memory.AsNoTracking()).Sort(type=> type.Name);
        }

        public Ram Ram { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public ObservableCollection<Types_memory> TypesMemories { get; }

        #region Команды

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Ram);
            RamViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Ram.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
