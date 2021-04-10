namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class ProcessorAddViewModel : BindableBase
    {
        public ProcessorAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking());
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking());
            Sockets = new ObservableCollection<Socket>(db.Sockets.AsNoTracking());
        }

        public Processor Processor { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Unit> Units { get; }

        public ObservableCollection<Socket> Sockets { get; }

        #region Команды

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Processor);
            ProcessorsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Processor.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
