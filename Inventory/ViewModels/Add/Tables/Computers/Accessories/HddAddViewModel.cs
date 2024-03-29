﻿namespace Inventory.ViewModels.Add.Tables.Computers.Accessories
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using Inventory.Services;
    using Inventory.ViewModels.Tables.Computers.Accessories;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Input;

    public class HddAddViewModel : BindableBase
    {
        public HddAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking()).Sort(manufact => manufact.Name);
            TypesHdd = new ObservableCollection<Types_hdd>(db.Types_hdd.AsNoTracking()).Sort(type => type.Name);
            Units = new ObservableCollection<Unit>(db.Units.AsNoTracking()).Sort(unit => unit.Full_name);
        }

        public Hdd Hdd { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_hdd> TypesHdd { get; }

        public ObservableCollection<Unit> Units { get; }

        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Hdd);
            HddViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Hdd.ErrorCollection));
    }
}
