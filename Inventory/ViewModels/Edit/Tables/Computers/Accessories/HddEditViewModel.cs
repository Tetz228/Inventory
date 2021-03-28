namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Model.Classes;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class HddEditViewModel : BindableBase
    {
        public HddEditViewModel(Hdd hdd)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers);
            TypesHdds = new ObservableCollection<Types_hdd>(db.Types_hdd);
            Units = new ObservableCollection<Unit>(db.Units);

            Hdd = hdd;
            BeginEdit();
        }

        public Hdd Hdd { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_hdd> TypesHdds { get; }

        public ObservableCollection<Unit> Units { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        #region Команды

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Hdd.Id_hdd, Hdd);
            HddsViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Hdd.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(editWindow =>
        {
            CancelEdit();
            editWindow.Close();
        });
        #endregion

        #region Откат изменений
        private Hdd _selectHdd;

        public void BeginEdit()
        {
            _selectHdd = new Hdd()
            {
                Id_hdd = Hdd.Id_hdd,
                Memory_size = Hdd.Memory_size,
                Name = Hdd.Name,
                Fk_type_hdd = Hdd.Fk_type_hdd,
                Fk_unit = Hdd.Fk_unit,
                Fk_manufacturer = Hdd.Fk_manufacturer,
            };
        }

        public void EndEdit()
        {
            _selectHdd = null;
        }

        public void CancelEdit()
        {
            if (_selectHdd == null)
                return;

            Hdd.Id_hdd = _selectHdd.Id_hdd;
            Hdd.Memory_size = _selectHdd.Memory_size;
            Hdd.Name = _selectHdd.Name;
            Hdd.Fk_type_hdd = _selectHdd.Fk_type_hdd;
            Hdd.Fk_unit = _selectHdd.Fk_unit;
            Hdd.Fk_manufacturer = _selectHdd.Fk_manufacturer;
        }
        #endregion
    }
}
