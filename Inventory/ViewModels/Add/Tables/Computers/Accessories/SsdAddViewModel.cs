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

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking());
            TypesSsds = new ObservableCollection<Types_ssd>(db.Types_ssd.AsNoTracking());
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking());
        }

        public Ssd Ssd { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_ssd> TypesSsds { get; }

        public ObservableCollection<Unit> Units { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Ssd);
            SsdViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Ssd.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
