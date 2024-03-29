﻿namespace Inventory.ViewModels.Edit.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class SsdEditViewModel : BindableBase, IEditableObject
    {
        public SsdEditViewModel(Ssd ssd)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            TypesSsds = new ObservableCollection<Types_ssd>(db.Types_ssd.AsNoTracking()).Sort(type => type.Name);
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking()).Sort(unit => unit.Full_name);

            Ssd = ssd;
            BeginEdit();
        }

        public Ssd Ssd { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_ssd> TypesSsds { get; }

        public ObservableCollection<Unit> Units { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Ssd.Id_ssd, Ssd);
            SsdViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Ssd.ErrorCollection));

        #region Откат изменений
        private Ssd _selectSsd;

        public void BeginEdit()
        {
            _selectSsd = new Ssd
            {
                Id_ssd = Ssd.Id_ssd,
                Memory_size = Ssd.Memory_size,
                Name = Ssd.Name,
                Fk_type_ssd = Ssd.Fk_type_ssd,
                Fk_unit = Ssd.Fk_unit,
                Fk_manufacturer = Ssd.Fk_manufacturer,
            };
        }

        public void EndEdit() => _selectSsd = null;

        public void CancelEdit()
        {
            if (_selectSsd == null)
                return;

            Ssd.Id_ssd = _selectSsd.Id_ssd;
            Ssd.Memory_size = _selectSsd.Memory_size;
            Ssd.Name = _selectSsd.Name;
            Ssd.Fk_type_ssd = _selectSsd.Fk_type_ssd;
            Ssd.Fk_unit = _selectSsd.Fk_unit;
            Ssd.Fk_manufacturer = _selectSsd.Fk_manufacturer;
        }
        #endregion
    }
}
