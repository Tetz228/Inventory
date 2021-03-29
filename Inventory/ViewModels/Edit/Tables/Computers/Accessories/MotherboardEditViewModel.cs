namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;

    public class MotherboardEditViewModel : BindableBase, IEditableObject
    {
        public MotherboardEditViewModel(Motherboard motherboard)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            Sockets = new ObservableCollection<Socket>(db.Sockets);

            Motherboard = motherboard;
            BeginEdit();
        }

        public Motherboard Motherboard { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Socket> Sockets { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Motherboard.Id_motherboard, Motherboard);
            MotherboardsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Motherboard.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Motherboard _selectMotherboard;

        public void BeginEdit()
        {
            _selectMotherboard = new Motherboard()
            {
                Id_motherboard = Motherboard.Id_motherboard,
                Name = Motherboard.Name,
                Fk_socket = Motherboard.Fk_socket,
                Fk_manufacturer = Motherboard.Fk_manufacturer,
            };
        }

        public void EndEdit()
        {
            _selectMotherboard = null;
        }

        public void CancelEdit()
        {
            if (_selectMotherboard == null)
                return;

            Motherboard.Id_motherboard = _selectMotherboard.Id_motherboard;
            Motherboard.Name = _selectMotherboard.Name;
            Motherboard.Fk_socket = _selectMotherboard.Fk_socket;
            Motherboard.Fk_manufacturer = _selectMotherboard.Fk_manufacturer;
        }
        #endregion
    }
}
