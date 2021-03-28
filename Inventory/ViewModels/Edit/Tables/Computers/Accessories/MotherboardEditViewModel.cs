namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DevExpress.Mvvm;

    using Inventory.Model;

    public class MotherboardEditViewModel : BindableBase
    {
        public MotherboardEditViewModel(Motherboard motherboard)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Sockets = new ObservableCollection<Socket>(db.Sockets);

            Motherboard = motherboard;
            Motherboard.BeginEdit();
        }

        public Motherboard Motherboard { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Socket> Sockets { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => Motherboard.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            Motherboard.EndEdit();
            Services.Edit(Motherboard.Id_motherboard, Motherboard);
            Motherboard.RefreshCollection();
            editWindow.Close();
        }, _ => Motherboard.IsValidationProperties());

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            Motherboard.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
