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

    public class HddEditViewModel : BindableBase, IEditableObject
    {
        public HddEditViewModel(Hdd hdd)
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            TypesHdd = new ObservableCollection<Types_hdd>(db.Types_hdd.AsNoTracking()).Sort(type => type.Name);
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking()).Sort(unit => unit.Full_name);

            Hdd = hdd;
            BeginEdit();
        }

        public Hdd Hdd { get; }

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_hdd> TypesHdd { get; }

        public ObservableCollection<Unit> Units { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => CancelEdit();

        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            EndEdit();
            Services.Edit(Hdd.Id_hdd, Hdd);
            HddViewModel.RefreshCollection();
            editWindow.Close();
        }, _ => Services.IsValidationProperties(Hdd.ErrorCollection));
        
        #region Откат изменений
        private Hdd _selectHdd;

        public void BeginEdit()
        {
            _selectHdd = new Hdd
            {
                Id_hdd = Hdd.Id_hdd,
                Memory_size = Hdd.Memory_size,
                Name = Hdd.Name,
                Fk_type_hdd = Hdd.Fk_type_hdd,
                Fk_unit = Hdd.Fk_unit,
                Fk_manufacturer = Hdd.Fk_manufacturer,
            };
        }

        public void EndEdit() => _selectHdd = null;

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
