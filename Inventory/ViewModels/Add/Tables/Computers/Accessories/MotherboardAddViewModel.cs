namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class MotherboardAddViewModel : BindableBase
    {
        public MotherboardAddViewModel()
        {
            using var db = new InventoryEntities();

            Motherboard = new Motherboard();
            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Sockets = new ObservableCollection<Socket>(db.Sockets);
        }

        public Motherboard Motherboard { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Socket> Sockets { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Motherboard);
            Motherboard.RefreshCollection();
            addWindow.Close();
        }, _ => Motherboard.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
