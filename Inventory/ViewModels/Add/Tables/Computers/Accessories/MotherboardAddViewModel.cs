namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class MotherboardAddViewModel : BindableBase
    {
        public MotherboardAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            Sockets = new ObservableCollection<Socket>(db.Sockets.AsNoTracking()).Sort(socket => socket.Name);
        }

        public Motherboard Motherboard { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Socket> Sockets { get; }

        #region Команды

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
                Services.Add(Motherboard);
                MotherboardsViewModel.RefreshCollection();
                addWindow.Close();
        }, _ => Services.IsValidationProperties(Motherboard.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
