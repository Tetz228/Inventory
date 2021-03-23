﻿namespace Inventory.ViewModels.Edit.Tables.Peripherals
{
    using DevExpress.Mvvm;
    using Inventory.Model;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    public class InventoryPeripheralEditViewModel : BindableBase
    {
        public InventoryPeripheralEditViewModel(Inventory_numbers_peripherals inventoryNumberPeripheral)
        {
            using var db = new InventoryEntities();

            Peripherals = new ObservableCollection<Peripheral>(db.Peripherals);
            StatusesPeripherals = new ObservableCollection<Statuses_peripherals>(db.Statuses_peripherals);
            InventoryNumberPeripheral = inventoryNumberPeripheral;
            InventoryNumberPeripheral.BeginEdit();
        }

        public Inventory_numbers_peripherals InventoryNumberPeripheral { get; }

        public ObservableCollection<Peripheral> Peripherals { get; }

        public ObservableCollection<Statuses_peripherals> StatusesPeripherals { get; }

        public void OnWindowClosing(object sender, CancelEventArgs e) => InventoryNumberPeripheral.CancelEdit();

        #region Команды
        public ICommand EditCommand => new DelegateCommand<Window>(editWindow =>
        {
            InventoryNumberPeripheral.EndEdit();
            Inventory_numbers_peripherals.EditInventoryNumber(InventoryNumberPeripheral);
            editWindow.Close();
        }, _ => InventoryNumberPeripheral.IsValidationProperties());

        public ICommand Cancel => new DelegateCommand<Window>(editWindow =>
        {
            InventoryNumberPeripheral.CancelEdit();
            editWindow.Close();
        });
        #endregion
    }
}
