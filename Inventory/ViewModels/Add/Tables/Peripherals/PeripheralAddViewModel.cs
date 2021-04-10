﻿namespace Inventory.ViewModels.Add.Tables.Peripherals
{
    using System.Collections.ObjectModel;

    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Windows;
    using System.Windows.Input;

    using Inventory.Services;
    using Inventory.ViewModels.Tables.Peripherals;

    public class PeripheralAddViewModel : BindableBase
    {
        public PeripheralAddViewModel()
        {
            using var db = new InventoryEntities();

            Manufacturers = new ObservableCollection<Manufacturer>(db.Manufacturers.AsNoTracking());
            TypesPeripherals = new ObservableCollection<Types_peripherals>(db.Types_peripherals.AsNoTracking());
        }

        public Peripheral Peripheral { get; } = new();

        public ObservableCollection<Manufacturer> Manufacturers { get; }

        public ObservableCollection<Types_peripherals> TypesPeripherals { get; }

        #region Команды
        public ICommand AddCommand => new DelegateCommand<Window>(addWindow =>
        {
            Services.Add(Peripheral);
            PeripheralsViewModel.RefreshCollection();
            addWindow.Close();
        }, _ => Services.IsValidationProperties(Peripheral.ErrorCollection));

        public ICommand CancelCommand => new DelegateCommand<Window>(addWindow => addWindow.Close());
        #endregion
    }
}
