namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class SsdAddViewModel : BindableBase
    {
        public SsdAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            TypesSsds = new ObservableCollection<Types_ssd>(db.Types_ssd.AsNoTracking()).Sort(type => type.Name);
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking()).Sort(unit => unit.Full_name);
        }

        public Ssd Ssd { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_ssd> TypesSsds { get; }

        public ObservableCollection<Unit> Units { get; }

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Ssd);
            SsdViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Ssd.ErrorCollection));
    }
}
