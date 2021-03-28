namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class HddAddViewModel : BindableBase
    {
        public HddAddViewModel()
        {
            using var db = new InventoryEntities();

            Hdd = new Hdd();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            TypesHdds = new ObservableCollection<Types_hdd>(db.Types_hdd);
            Units = new ObservableCollection<Unit>(db.Units);
        }

        public Hdd Hdd { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_hdd> TypesHdds { get; }

        public ObservableCollection<Unit> Units { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Hdd);
            Hdd.RefreshCollection();
            addWindow.Close();
        }, _ => Hdd.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
